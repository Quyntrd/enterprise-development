using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Generator.RabbitMq.Host.Generators;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Generator strategy for rental DTOs.
/// </summary>
public sealed class RentalGeneratorStrategy : IGeneratorStrategy<RentalCreateUpdateDto>
{
    /// <inheritdoc />
    public IList<RentalCreateUpdateDto> Generate(int count) =>
        RentalGenerator.GenerateRentals(count);

    /// <inheritdoc />
    public Task PublishAsync(
        IProducerService producer,
        IList<RentalCreateUpdateDto> batch
    ) =>
        producer.SendRentalsAsync(batch);
}
