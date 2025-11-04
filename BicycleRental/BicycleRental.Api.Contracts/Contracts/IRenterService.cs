using BicycleRental.Api.Contracts.Rentals;

namespace BicycleRental.Api.Contracts.Contracts;

/// <summary>
/// Application service for renters (clients).
/// </summary>
public interface IRenterService : IApplicationService<
    Renters.RenterDto,
    Renters.RenterCreateUpdateDto,
    int>
{
    /// <summary>
    /// Gets rentals for the specified renter.
    /// </summary>
    /// <param name="dtoId">Renter identifier</param>
    /// <returns>List of RentalDto</returns>
    List<RentalDto> GetRentals(int dtoId);
}
