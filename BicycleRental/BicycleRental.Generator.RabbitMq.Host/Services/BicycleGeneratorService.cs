using BicycleRental.Generator.RabbitMq.Host.Generators;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Background service that generates bicycle instances and publishes them.
/// </summary>
public class BicycleGeneratorService(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<BicycleGeneratorService> logger) : BackgroundService
{
    private readonly string _batchSize = configuration.GetSection("Generator:Bicycle")["BatchSize"] ?? throw new KeyNotFoundException("BatchSize section of Generator:Bicycle is missing");
    private readonly string _payloadLimit = configuration.GetSection("Generator:Bicycle")["PayloadLimit"] ?? throw new KeyNotFoundException("PayloadLimit section of Generator:Bicycle is missing");
    private readonly string _waitTime = configuration.GetSection("Generator:Bicycle")["WaitTime"] ?? throw new KeyNotFoundException("WaitTime section of Generator:Bicycle is missing");

    /// <summary>
    /// Executes the generator loop, producing batches of clients
    /// until the payload limit is reached or cancellation is requested
    /// </summary>
    /// <param name="stoppingToken">Token used to stop the background service</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("BicycleGeneratorService started with {batch} batch size, {limit} payload limit, {wait}s wait time", _batchSize, _payloadLimit, _waitTime);

        if (!int.TryParse(_batchSize, out var batchSize)) throw new FormatException("Unable to parse BatchSize");
        if (!int.TryParse(_payloadLimit, out var payloadLimit)) throw new FormatException("Unable to parse PayloadLimit");
        if (!int.TryParse(_waitTime, out var waitTime)) throw new FormatException("Unable to parse WaitTime");

        var counter = 0;
        using var scope = scopeFactory.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducerService>();

        while (counter < payloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var bikes = BicycleGenerator.GenerateBicycles(batchSize);
            await producer.SendBicyclesAsync(bikes);

            await Task.Delay(waitTime * 1000, stoppingToken);
            counter += batchSize;
        }

        logger.LogInformation("BicycleGeneratorService finished sending {total} messages", counter);
    }
}