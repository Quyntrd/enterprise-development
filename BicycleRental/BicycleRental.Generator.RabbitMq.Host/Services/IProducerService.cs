using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;

namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Producer contract for sending generated DTO batches to RabbitMQ.
/// </summary>
public interface IProducerService
{
    /// <summary>
    /// Publishes a batch of bicycle model records
    /// </summary>
    /// <param name="batch">List of client DTOs to send</param>
    public Task SendBicycleModelsAsync(IList<BicycleModelCreateUpdateDto> batch);

    /// <summary>
    /// Publishes a batch of bicycle records
    /// </summary>
    /// <param name="batch">List of client DTOs to send</param>
    public Task SendBicyclesAsync(IList<BicycleCreateUpdateDto> batch);

    /// <summary>
    /// Publishes a batch of renter records
    /// </summary>
    /// <param name="batch">List of client DTOs to send</param>
    public Task SendRentersAsync(IList<RenterCreateUpdateDto> batch);

    /// <summary>
    /// Publishes a batch of rental records
    /// </summary>
    /// <param name="batch">List of client DTOs to send</param>
    public Task SendRentalsAsync(IList<RentalCreateUpdateDto> batch);
}