using BicycleRental.Application.Contracts.Renters;
using BicycleRental.Generator.RabbitMq.Host.Generators;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Generator strategy for renter DTOs.
/// </summary>
public sealed class RenterGeneratorStrategy : IGeneratorStrategy<RenterCreateUpdateDto>
{
    /// <inheritdoc />
    public IList<RenterCreateUpdateDto> Generate(int count) =>
        RenterGenerator.GenerateRenters(count);

    /// <inheritdoc />
    public Task PublishAsync(
        IProducerService producer,
        IList<RenterCreateUpdateDto> batch
    ) =>
        producer.SendRentersAsync(batch);
}
