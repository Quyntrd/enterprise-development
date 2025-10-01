using BicycleRental.Domain.Enums;

namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a specific <see cref="BicycleModel"/>  (e.g. "SportPro 1000").
/// </summary>
public class BicycleModel
{
    /// <summary>
    /// Unique identifier of the <see cref="BicycleModel"/>.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Name of the <see cref="BicycleModel"/>. (e.g. "SportPro 1000").
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The <see cref="BicycleType"/> this <see cref="BicycleModel"/> belongs to.
    /// </summary>
    public required BicycleType Type { get; set; }

    /// <summary>
    /// Wheel size in inches of the <see cref="BicycleModel"/>.
    /// </summary>
    public required double WheelSizeInInches { get; set; }

    /// <summary>
    /// Max passenger weight in kilograms of the <see cref="BicycleModel"/>.
    /// </summary>
    public required double MaxPassengerWeightKg { get; set; }

    /// <summary>
    /// Weight in kilograms of the <see cref="BicycleModel"/>.
    /// </summary>
    public required double WeightKg { get; set; }

    /// <summary>
    /// Type of brakes of the <see cref="BicycleModel"/>.
    /// </summary>
    public required string BrakeType { get; set; }

    /// <summary>
    /// Model year of the <see cref="BicycleModel"/>.
    /// </summary>
    public required int ModelYear { get; set; }

    /// <summary>
    /// Price for rental per hour of the <see cref="BicycleModel"/>.
    /// </summary>
    public required decimal PricePerHour { get; set; }
}
