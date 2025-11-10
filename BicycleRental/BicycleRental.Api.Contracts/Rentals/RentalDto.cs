namespace BicycleRental.Api.Contracts.Rentals;
/// <summary>
/// DTO for GET responses for rentals.
/// PricePerHour and TotalPrice are returned to client (calculated by service).
/// </summary>
/// <param name="Id">Identifier of the rental</param>
/// <param name="BicycleId">Identifier of bicycle</param>
/// <param name="RenterId">Identifier of renter</param>
/// <param name="StartAt">Start date/time</param>
/// <param name="DurationHours">Duration</param>
/// <param name="PricePerHour">Price per hour at moment of rental (returned for convenience)</param>
/// <param name="TotalPrice">Total price (rounded to 2 decimals)</param>
public record RentalDto(
    int Id,
    int BicycleId,
    int RenterId,
    DateTime StartAt,
    TimeSpan DurationHours,
    decimal PricePerHour = 0m,
    decimal TotalPrice = 0m);