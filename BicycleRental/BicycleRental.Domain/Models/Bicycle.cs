using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a bicycle instance (physical bike).
/// </summary>
[Table("bicycles")]
public class Bicycle
{
    /// <summary>
    /// Unique identifier of the Bicycle.
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// Serial number of the Bicycle (e.g. "SN-1001").
    /// </summary>
    [Required]
    [StringLength(100)]
    [Column("serial_number")]
    public required string SerialNumber { get; set; }

    /// <summary>
    /// The BicycleModel identifier for this bicycle.
    /// </summary>
    [Required]
    [Column("model_id")]
    public required int ModelId { get; set; }

    /// <summary>
    /// Color of the Bicycle.
    /// </summary>
    [StringLength(50)]
    [Column("color")]
    public string? Color { get; set; }
}