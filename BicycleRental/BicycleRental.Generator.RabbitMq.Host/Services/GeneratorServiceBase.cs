using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Generic background service that executes a generator strategy
/// using a unified generation and publishing loop.
/// </summary>
/// <typeparam name="T">DTO type being generated</typeparam>
/// <remarks>
/// Initializes the generator service with unified parameters from configuration.
/// </remarks>
/// <param name="configuration">Application configuration containing "Parameters"</param>
/// <param name="scopeFactory">Scope factory for resolving services</param>
/// <param name="logger">Logger instance</param>
/// <param name="strategy">Generator strategy for DTO type T</param>
public sealed class GeneratorServiceBase<T>(
    IConfiguration configuration,
    IServiceScopeFactory scopeFactory,
    ILogger<GeneratorServiceBase<T>> logger,
    IGeneratorStrategy<T> strategy
    ) : BackgroundService
{
    private readonly int _batchSize = int.Parse(configuration["Parameters:GeneratorBatchSize"] ?? "10");
    private readonly int _payloadLimit = int.Parse(configuration["Parameters:GeneratorPayloadLimit"] ?? "100");
    private readonly int _waitTime = int.Parse(configuration["Parameters:GeneratorWaitTime"] ?? "1");

    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly ILogger<GeneratorServiceBase<T>> _logger = logger;
    private readonly IGeneratorStrategy<T> _strategy = strategy;

    /// <summary>
    /// Executes the background generation loop until the payload limit
    /// is reached or cancellation is requested.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{service} started with batch={batch}, limit={limit}, wait={wait}s",
            typeof(T).Name,
            _batchSize,
            _payloadLimit,
            _waitTime
        );

        var counter = 0;

        using var scope = _scopeFactory.CreateScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducerService>();

        while (counter < _payloadLimit && !stoppingToken.IsCancellationRequested)
        {
            var batch = _strategy.Generate(_batchSize);
            await _strategy.PublishAsync(producer, batch);

            counter += batch.Count;
            await Task.Delay(_waitTime * 1000, stoppingToken);
        }

        _logger.LogInformation(
            "{service} finished sending {total} messages",
            typeof(T).Name,
            counter
        );
    }
}
