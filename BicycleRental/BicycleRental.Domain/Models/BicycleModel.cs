using BicycleRental.Domain.Enums;

namespace BicycleRental.Domain.Models;

/// <summary>
/// 
/// </summary>
public class BicycleModel
{
    /// <summary>
    /// 
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required BicycleType Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required double WheelSizeInInches { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required double MaxPassengerWeightKg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required double WeightKg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string BrakeType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int ModelYear { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required decimal PricePerHour { get; set; }
}
