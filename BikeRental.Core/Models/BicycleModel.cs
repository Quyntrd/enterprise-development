using BikeRental.Domain.Enums;

namespace BikeRental.Domain.Models;

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
    public required int WheelSizeInInches { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required decimal MaxPassengerWeightKg { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required decimal WeightKg { get; set; }

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
