namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a rental contract: a single instance of a bicycle rented by a renter.
/// </summary>
public class Rental
{
    /// <summary>
    /// Unique identifier of the <see cref="Rental"/>.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Identifier of the rented <see cref="Bicycle"/> (foreign key).
    /// </summary>
    public required int BicycleId { get; set; }

    /// <summary>
    /// Identifier of the <see cref="Renter"/> who took the bicycle (foreign key).
    /// </summary>
    public required int RenterId { get; set; }

    /// <summary>
    /// Date and time when the rental starts.
    /// </summary>
    public required DateTime StartAt { get; set; }

    /// <summary>
    /// Duration of the rental in hours. May be fractional (for example, 1.5 means one hour and thirty minutes).
    /// </summary>
    public required decimal DurationHours { get; set; }

    /// <summary>
    /// Price per hour recorded at the moment of rental (copied from the <see cref="BicycleModel.PricePerHour"/>).
    /// Used to calculate revenue for this rental regardless of later price changes.
    /// </summary>
    public required decimal PricePerHourAtRental { get; set; }
}
