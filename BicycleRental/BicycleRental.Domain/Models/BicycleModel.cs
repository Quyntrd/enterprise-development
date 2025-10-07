using BicycleRental.Domain.Enums;

namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a specific Bicycle Model  (e.g. "SportPro 1000").
/// </summary>
public class BicycleModel
{
    /// <summary>
    /// Unique identifier of the Bicycle Model.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Name of the Bicycle Model. (e.g. "SportPro 1000").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The <see cref="BicycleType"/> this Bicycle Model belongs to.
    /// </summary>
    public required BicycleType Type { get; set; }

    /// <summary>
    /// Wheel size in inches of the Bicycle Model.
    /// </summary>
    public double? WheelSizeInInches { get; set; }

    /// <summary>
    /// Max passenger weight in kilograms of the Bicycle Model.
    /// </summary>
    public double? MaxPassengerWeightKg { get; set; }

    /// <summary>
    /// Weight in kilograms of the Bicycle Model.
    /// </summary>
    public double? WeightKg { get; set; }

    /// <summary>
    /// Type of brakes of the Bicycle Model.
    /// </summary>
    public string? BrakeType { get; set; }

    /// <summary>
    /// Model year of the Bicycle Model.
    /// </summary>
    public int? ModelYear { get; set; }

    /// <summary>
    /// Price for rental per hour of the Bicycle Model.
    /// </summary>
    public required decimal PricePerHour { get; set; }
}
