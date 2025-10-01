namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a o.g. <see cref="Bicycle"/>.
/// </summary>
public class Bicycle
{
    /// <summary>
    /// Unique identifier of the <see cref="Bicycle"/>.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Serial number of the <see cref="Bicycle"/> (e.g. "SN-1001").
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// The <see cref="BicycleModel"/> identifier of o.g. <see cref="Bicycle"/>.
    /// </summary>
    public required int ModelId { get; set; }

    /// <summary>
    /// Color of the <see cref="Bicycle"/>.
    /// </summary>
    public required string Color { get; set; }
}