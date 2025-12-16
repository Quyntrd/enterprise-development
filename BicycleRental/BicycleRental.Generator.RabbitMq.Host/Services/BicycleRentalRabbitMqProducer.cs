using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;
using RabbitMQ.Client;
using System.Text.Json;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Producer service that publishes generated DTO batches to RabbitMQ.
/// </summary>
/// <param name="configuration">Application configuration providing RabbitMQ settings.</param>
/// <param name="rabbitMqConnection">RabbitMQ connection used to create channels.</param>
/// <param name="logger">Logger instance for diagnostic output.</param>
public class BicycleRentalRabbitMqProducer(
    IConfiguration configuration,
    IConnection rabbitMqConnection,
    ILogger<BicycleRentalRabbitMqProducer> logger
) : IProducerService, IDisposable
{
    private readonly string _queueName =
        configuration.GetSection("RabbitMq")["QueueName"]
        ?? throw new KeyNotFoundException("RabbitMq:QueueName section is missing in configuration.");

    private const string ExchangeName = "bicycle.exchange";

    private readonly IModel _channel = rabbitMqConnection.CreateModel();

    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Ensures that the required RabbitMQ exchange and queue exist
    /// and binds all relevant routing keys.
    /// This method is idempotent and may be called prior to publishing.
    /// </summary>
    private void EnsureExchangeAndQueue()
    {
        _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct, durable: true);

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(_queueName, ExchangeName, "bicyclemodel");
        _channel.QueueBind(_queueName, ExchangeName, "bicycle");
        _channel.QueueBind(_queueName, ExchangeName, "renter");
        _channel.QueueBind(_queueName, ExchangeName, "rental");
    }

    /// <summary>
    /// Publishes a batch of bicycle model DTOs to RabbitMQ using the "bicyclemodel" routing key.
    /// </summary>
    /// <param name="batch">List of <see cref="BicycleModelCreateUpdateDto"/> to send.</param>
    public Task SendBicycleModelsAsync(IList<BicycleModelCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch, _jsonOptions);
        var props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2; // persistent

        _channel.BasicPublish(ExchangeName, "bicyclemodel", props, payload);

        logger.LogInformation("Sent {count} bicycle models", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publishes a batch of bicycle DTOs to RabbitMQ using the "bicycle" routing key.
    /// </summary>
    /// <param name="batch">List of <see cref="BicycleCreateUpdateDto"/> to send.</param>
    public Task SendBicyclesAsync(IList<BicycleCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch, _jsonOptions);
        var props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        _channel.BasicPublish(ExchangeName, "bicycle", props, payload);

        logger.LogInformation("Sent {count} bicycles", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publishes a batch of renter DTOs to RabbitMQ using the "renter" routing key.
    /// </summary>
    /// <param name="batch">List of <see cref="RenterCreateUpdateDto"/> to send.</param>
    public Task SendRentersAsync(IList<RenterCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch, _jsonOptions);
        var props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        _channel.BasicPublish(ExchangeName, "renter", props, payload);

        logger.LogInformation("Sent {count} renters", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Publishes a batch of rental DTOs to RabbitMQ using the "rental" routing key.
    /// </summary>
    /// <param name="batch">List of <see cref="RentalCreateUpdateDto"/> to send.</param>
    public Task SendRentalsAsync(IList<RentalCreateUpdateDto> batch)
    {
        EnsureExchangeAndQueue();

        var payload = JsonSerializer.SerializeToUtf8Bytes(batch, _jsonOptions);
        var props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        _channel.BasicPublish(ExchangeName, "rental", props, payload);

        logger.LogInformation("Sent {count} rentals", batch.Count);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes the underlying RabbitMQ channel and logs disposal.
    /// </summary>
    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        logger.LogInformation("RabbitMQ Producer disposed");
    }
}
