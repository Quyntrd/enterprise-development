using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;
using BicycleRental.Application.Contracts.Contracts;

namespace BicycleRental.Infrastructure.RabbitMq;

/// <summary>
/// Background service responsible for consuming messages from RabbitMQ,
/// deserializing them and delegating processing to the appropriate scoped application services.
/// </summary>
public class BicycleRentalRabbitMqConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BicycleRentalRabbitMqConsumer> _logger;
    private readonly string _queueName;
    private IModel? _channel;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Creates new instance of <see cref="BicycleRentalRabbitMqConsumer"/>.
    /// </summary>
    /// <param name="connection">RabbitMQ connection injected from DI.</param>
    /// <param name="scopeFactory">Factory to create scopes for scoped services.</param>
    /// <param name="configuration">Configuration to read RabbitMq settings from.</param>
    /// <param name="logger">Logger for diagnostics.</param>
    public BicycleRentalRabbitMqConsumer(
        IConnection connection,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<BicycleRentalRabbitMqConsumer> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _queueName = configuration.GetSection("RabbitMq")["QueueName"]
            ?? throw new KeyNotFoundException("RabbitMq:QueueName section is missing in configuration.");
    }

    /// <summary>
    /// Initializes RabbitMQ channel and begins consuming messages.
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Establishing RabbitMQ channel to queue '{queue}'", _queueName);

        stoppingToken.ThrowIfCancellationRequested();

        _channel = _connection.CreateModel();

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) => await ReceiveMessageAsync(ea, stoppingToken);

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        _logger.LogInformation("Started listening to queue '{queue}'", _queueName);
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
                    await ProcessBicycleModelAsync(json, scope);
                    break;

                case "bicycle":
                case "bicycle.create":
                case "bicycle.update":
                    await ProcessBicycleAsync(json, scope);
                    break;

                case "renter":
                case "renter.create":
                    await ProcessRenterAsync(json, scope);
                    break;

                case "rental":
                case "rental.create":
                case "rental.update":
                    await ProcessRentalAsync(json, scope);
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
                _channel?.BasicNack(deliveryTag: args.DeliveryTag, multiple: false, requeue: false);
            }
            catch (Exception nackEx)
            {
                _logger.LogError(nackEx, "Failed to Nack message for deliveryTag {deliveryTag}", args.DeliveryTag);
            }
        }
    }

    private async Task ProcessBicycleModelAsync(string json, IServiceScope scope)
    {
        var svc = scope.ServiceProvider.GetRequiredService<IBicycleModelService>();

        if (TryDeserialize<List<BicycleModelCreateUpdateDto>>(json, out var list) && list is not null)
        {
            foreach (var dto in list)
            {
                await svc.Create(dto);
            }

            return;
        }

        var single = JsonSerializer.Deserialize<BicycleModelCreateUpdateDto>(json, _jsonOptions)
            ?? throw new FormatException("Unable to deserialize BicycleModelCreateUpdateDto from message body");

        await svc.Create(single);
    }

    private async Task ProcessBicycleAsync(string json, IServiceScope scope)
    {
        var svc = scope.ServiceProvider.GetRequiredService<IBicycleService>();

        if (TryDeserialize<List<BicycleCreateUpdateDto>>(json, out var list) && list is not null)
        {
            foreach (var dto in list)
            {
                await svc.Create(dto);
            }

            return;
        }

        var single = JsonSerializer.Deserialize<BicycleCreateUpdateDto>(json, _jsonOptions)
            ?? throw new FormatException("Unable to deserialize BicycleCreateUpdateDto from message body");

        await svc.Create(single);
    }

    private async Task ProcessRenterAsync(string json, IServiceScope scope)
    {
        var svc = scope.ServiceProvider.GetRequiredService<IRenterService>();

        if (TryDeserialize<List<RenterCreateUpdateDto>>(json, out var list) && list is not null)
        {
            foreach (var dto in list)
            {
                await svc.Create(dto);
            }

            return;
        }

        var single = JsonSerializer.Deserialize<RenterCreateUpdateDto>(json, _jsonOptions)
            ?? throw new FormatException("Unable to deserialize RenterCreateUpdateDto from message body");

        await svc.Create(single);
    }

    private async Task ProcessRentalAsync(string json, IServiceScope scope)
    {
        var svc = scope.ServiceProvider.GetRequiredService<IRentalService>();

        if (TryDeserialize<List<RentalCreateUpdateDto>>(json, out var list) && list is not null)
        {
            foreach (var dto in list)
            {
                await svc.Create(dto);
            }

            return;
        }

        var single = JsonSerializer.Deserialize<RentalCreateUpdateDto>(json, _jsonOptions)
            ?? throw new FormatException("Unable to deserialize RentalCreateUpdateDto from message body");

        await svc.Create(single);
    }

    /// <summary>
    /// Tries to deserialize json into T. Returns false on error without throwing.
    /// Useful to detect whether payload is an array/list.
    /// </summary>
    private bool TryDeserialize<T>(string json, out T? value)
    {
        try
        {
            value = JsonSerializer.Deserialize<T>(json, _jsonOptions);
            return value is not null;
        }
        catch
        {
            value = default;
            return false;
        }
    }
}
