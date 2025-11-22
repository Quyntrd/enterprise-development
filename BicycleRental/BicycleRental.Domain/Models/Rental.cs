using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a rental contract: a single instance of a bicycle rented by a renter.
/// </summary>
[Table("rentals")]
public class Rental
{
    /// <summary>
    /// Unique identifier of the Rental.
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Identifier of the rented <see cref="Bicycle"/> (foreign key).
    /// </summary>
    [Required]
    [Column("bicycle_id")]
    public required int BicycleId { get; set; }

    /// <summary>
    /// Identifier of the <see cref="Renter"/> who took the bicycle (foreign key).
    /// </summary>
    [Required]
    [Column("renter_id")]
    public required int RenterId { get; set; }

    /// <summary>
    /// Date and time when the rental starts.
    /// </summary>
    [Required]
    [Column("start_at")]
    public required DateTime StartAt { get; set; }

    /// <summary>
    /// Duration of the rental (TimeSpan). Mapped to appropriate DB type (interval/time span).
    /// </summary>
    [Required]
    [Column("duration_hours")]
    public required TimeSpan DurationHours { get; set; }
}
