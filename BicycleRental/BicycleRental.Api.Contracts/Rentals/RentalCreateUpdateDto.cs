namespace BicycleRental.Api.Contracts.Rentals;
/// <summary>
/// DTO for POST/PUT requests for rentals
/// </summary>
/// <param name="BicycleId">Identifier of the bicycle being rented</param>
/// <param name="RenterId">Identifier of the renter</param>
/// <param name="StartAt">Start date and time of rental</param>
/// <param name="DurationHours">Duration of rental (TimeSpan)</param>
public record RentalCreateUpdateDto(
    int BicycleId,
    int RenterId,
    DateTime StartAt,
    TimeSpan DurationHours);
