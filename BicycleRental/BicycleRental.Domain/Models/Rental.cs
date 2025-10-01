namespace BicycleRental.Domain.Models;

/// <summary>
/// 
/// </summary>
public class Rental
{
    /// <summary>
    ///
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required int BicycleId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required int RenterId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required DateTime StartAt { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required decimal DurationHours { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required decimal PricePerHourAtRental { get; set; }
}
