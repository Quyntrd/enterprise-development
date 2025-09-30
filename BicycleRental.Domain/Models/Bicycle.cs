namespace BicycleRental.Domain.Models;

/// <summary>
/// 
/// </summary>
public class Bicycle
{
    /// <summary>
    /// 
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