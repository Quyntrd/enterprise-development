using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Generator.RabbitMq.Host.Generators;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Generator strategy for bicycle DTOs.
/// </summary>
public sealed class BicycleGeneratorStrategy : IGeneratorStrategy<BicycleCreateUpdateDto>
{
    /// <inheritdoc />
    public IList<BicycleCreateUpdateDto> Generate(int count) =>
        BicycleGenerator.GenerateBicycles(count);

    /// <inheritdoc />
    public Task PublishAsync(
        IProducerService producer,
        IList<BicycleCreateUpdateDto> batch
    ) =>
        producer.SendBicyclesAsync(batch);
}
