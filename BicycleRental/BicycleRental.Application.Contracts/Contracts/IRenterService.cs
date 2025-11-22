using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;

namespace BicycleRental.Application.Contracts.Contracts;

/// <summary>
/// Application service for renters (clients).
/// </summary>
public interface IRenterService : IApplicationService<
    RenterDto,
    RenterCreateUpdateDto,
    int>
{
    /// <summary>
    /// Gets rentals for the specified renter.
    /// </summary>
    /// <param name="dtoId">Renter identifier</param>
    /// <returns>List of RentalDto</returns>
    public Task<List<RentalDto>> GetRentals(int dtoId);
}
