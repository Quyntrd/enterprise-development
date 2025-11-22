using BicycleRental.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a specific bicycle model (e.g. "SportPro 1000").
/// </summary>
[Table("bicycle_models")]
public class BicycleModel
{
    /// <summary>
    /// Unique identifier of the Bicycle Model.
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Name of the Bicycle Model (e.g. "SportPro 1000").
    /// </summary>
    [Required]
    [StringLength(200)]
    [Column("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The <see cref="BicycleType"/> this Bicycle Model belongs to.
    /// Stored as integer in the database.
    /// </summary>
    [Required]
    [Column("type")]
    public required BicycleType Type { get; set; }

    /// <summary>
    /// Wheel size in inches of the Bicycle Model.
    /// </summary>
    [Column("wheel_size_in_inches")]
    public double? WheelSizeInInches { get; set; }

    /// <summary>
    /// Max passenger weight in kilograms of the Bicycle Model.
    /// </summary>
    [Column("max_passenger_weight_kg")]
    public double? MaxPassengerWeightKg { get; set; }

    /// <summary>
    /// Weight in kilograms of the Bicycle Model.
    /// </summary>
    [Column("weight_kg")]
    public double? WeightKg { get; set; }

    /// <summary>
    /// Type of brakes of the Bicycle Model.
    /// </summary>
    [StringLength(100)]
    [Column("brake_type")]
    public string? BrakeType { get; set; }

    /// <summary>
    /// Model year of the Bicycle Model.
    /// </summary>
    [Column("model_year")]
    public int? ModelYear { get; set; }

    /// <summary>
    /// Price for rental per hour of the Bicycle Model.
    /// </summary>
    [Required]
    [Column("price_per_hour", TypeName = "numeric")]
    public required decimal PricePerHour { get; set; }
}
