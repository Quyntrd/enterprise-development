namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a o.g. Bicycle
/// </summary>
public class Bicycle
{
    /// <summary>
    /// Unique id of the Bicycle
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required int ModelId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Color { get; set; }
}