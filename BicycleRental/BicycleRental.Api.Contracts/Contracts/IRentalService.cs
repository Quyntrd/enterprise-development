using BicycleRental.Api.Contracts.Rentals;

namespace BicycleRental.Api.Contracts.Contracts;

/// <summary>
/// Application service for rentals.
/// </summary>
public interface IRentalService : IApplicationService<
    RentalDto,
    RentalCreateUpdateDto,
    int>
{
    /// <summary>
    /// Get rentals by bicycle id
    /// </summary>
    /// <param name="bicycleId">Bicycle identifier</param>
    /// <returns>List of RentalDto</returns>
    List<RentalDto> GetByBicycleId(int bicycleId);

    /// <summary>
    /// Get rentals by renter id
    /// </summary>
    /// <param name="renterId">Renter identifier</param>
    /// <returns>List of RentalDto</returns>
    List<RentalDto> GetByRenterId(int renterId);
}
