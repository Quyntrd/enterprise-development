namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a rental contract: a single instance of a bicycle rented by a renter.
/// </summary>
public class Rental
{
    /// <summary>
    /// Unique identifier of the Rental.
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
    /// Duration of the rental in hours.
    /// </summary>
    public required TimeSpan DurationHours { get; set; }
}
