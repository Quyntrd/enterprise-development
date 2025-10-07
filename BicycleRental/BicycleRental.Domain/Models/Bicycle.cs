namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a o.g. Bicycle.
/// </summary>
public class Bicycle
{
    /// <summary>
    /// Unique identifier of the Bicycle.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Serial number of the Bicycle (e.g. "SN-1001").
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// The <see cref="BicycleModel"/> identifier of o.g. Bicycle.
    /// </summary>
    public required int ModelId { get; set; }

    /// <summary>
    /// Color of the Bicycle.
    /// </summary>
    public string? Color { get; set; }
}