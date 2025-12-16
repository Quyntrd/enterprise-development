using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BicycleRental.Infrastructure.RabbitMq;

/// <summary>
/// Background service responsible for consuming messages from RabbitMQ,
/// deserializing them and delegating processing to the appropriate scoped application services.
/// </summary>
/// <remarks>
/// Creates new instance of <see cref="BicycleRentalRabbitMqConsumer"/>.
/// </remarks>
/// <param name="connection">RabbitMQ connection injected from DI.</param>
/// <param name="scopeFactory">Factory to create scopes for scoped services.</param>
/// <param name="configuration">Configuration to read RabbitMq settings from.</param>
/// <param name="logger">Logger for diagnostics.</param>
public class BicycleRentalRabbitMqConsumer(
    IConnection connection,
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<BicycleRentalRabbitMqConsumer> logger) : BackgroundService
{
    private readonly IConnection _connection = connection ?? throw new ArgumentNullException(nameof(connection));
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    private readonly ILogger<BicycleRentalRabbitMqConsumer> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly string _queueName = configuration.GetSection("RabbitMq")["QueueName"]
            ?? throw new KeyNotFoundException("RabbitMq:QueueName section is missing in configuration.");
    private IModel? _channel;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes RabbitMQ channel and begins consuming messages.
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Establishing RabbitMQ channel to queue '{queue}'", _queueName);

        stoppingToken.ThrowIfCancellationRequested();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        _logger.LogInformation("Started listening to queue '{queue}'", _queueName);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) => await ReceiveMessageAsync(ea, stoppingToken);

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gracefully closes channel and connection when stopping.
    /// </summary>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while closing RabbitMQ channel");
        }

        try
        {
            if (_connection?.IsOpen == true)
            {
                _connection.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error while closing RabbitMQ connection");
        }

        return base.StopAsync(cancellationToken);
    }

    /// <summary>
    /// Handles a received delivery: deserializes payload and routes to appropriate processor.
    /// Manual ack/nack is used: ack on success, nack (no requeue) on failure.
    /// </summary>
    private async Task ReceiveMessageAsync(BasicDeliverEventArgs args, CancellationToken stoppingToken)
    {
        var routingKey = args.RoutingKey ?? string.Empty;
        _logger.LogInformation("Received message from queue '{queue}' with routing key '{routingKey}'", _queueName, routingKey);

        try
        {
            stoppingToken.ThrowIfCancellationRequested();

            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            using var scope = _scopeFactory.CreateScope();

            switch (routingKey)
            {
                case "bicyclemodel":
                case "bicycle.model":
                case "bicycle.model.create":
                    var modelSvc = scope.ServiceProvider.GetRequiredService<IBicycleModelService>();
                    await ProcessBatchOrSingleAsync<BicycleModelCreateUpdateDto>(json, dto => modelSvc.Create(dto));
                    break;

                case "bicycle":
                case "bicycle.create":
                case "bicycle.update":
                    var bikeSvc = scope.ServiceProvider.GetRequiredService<IBicycleService>();
                    await ProcessBatchOrSingleAsync<BicycleCreateUpdateDto>(json, dto => bikeSvc.Create(dto));
                    break;

                case "renter":
                case "renter.create":
                    var renterSvc = scope.ServiceProvider.GetRequiredService<IRenterService>();
                    await ProcessBatchOrSingleAsync<RenterCreateUpdateDto>(json, dto => renterSvc.Create(dto));
                    break;

                case "rental":
                case "rental.create":
                case "rental.update":
                    var rentalSvc = scope.ServiceProvider.GetRequiredService<IRentalService>();
                    await ProcessBatchOrSingleAsync<RentalCreateUpdateDto>(json, dto => rentalSvc.Create(dto));
                    break;

                default:
                    _logger.LogWarning("Unknown routing key: {routingKey}. Message will be acked to avoid stuck queue.", routingKey);
                    _channel?.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                    return;
            }

            _channel?.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            _logger.LogInformation("Processed and acknowledged message with routing key '{routingKey}'", routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while processing message from queue '{queue}' with routing key '{routingKey}'", _queueName, routingKey);
            try
            {
                if (_channel?.IsOpen == true)
                {
                    _channel.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: false);
                }
            }
            catch (Exception nackEx)
            {
                _logger.LogError(nackEx, "Failed to Nack message for deliveryTag {deliveryTag}", args.DeliveryTag);
            }
        }
    }

    private async Task ProcessBatchOrSingleAsync<T>(string json, Func<T, Task> action)
    {
        var trimmedJson = json.TrimStart();

        if (trimmedJson.StartsWith("["))
        {
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            if (list is not null)
            {
                foreach (var item in list)
                {
                    try
                    {
                        await action(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process item of type {Type} in batch.", typeof(T).Name);
                    }
                }
            }
        }
        else
        {
            var single = JsonSerializer.Deserialize<T>(json, _jsonOptions)
                    ?? throw new FormatException($"Unable to deserialize {typeof(T).Name} from message body");

            await action(single);
        }
    }
}