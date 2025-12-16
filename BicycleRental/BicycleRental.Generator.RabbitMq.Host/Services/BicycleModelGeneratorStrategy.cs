using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Generator.RabbitMq.Host.Generators;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Generator strategy for bicycle model DTOs.
/// </summary>
public sealed class BicycleModelGeneratorStrategy : IGeneratorStrategy<BicycleModelCreateUpdateDto>
{
    /// <inheritdoc />
    public IList<BicycleModelCreateUpdateDto> Generate(int count) =>
        BicycleModelGenerator.GenerateModels(count);

    /// <inheritdoc />
    public Task PublishAsync(
        IProducerService producer,
        IList<BicycleModelCreateUpdateDto> batch
    ) =>
        producer.SendBicycleModelsAsync(batch);
}
