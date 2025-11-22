using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a client who can rent bicycles from the rental point.
/// </summary>
[Table("renters")]
public class Renter
{
    /// <summary>
    /// Unique identifier of the Renter.
    /// </summary>
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    /// <summary>
    /// First name of the Renter.
    /// </summary>
    [Required]
    [StringLength(30)]
    [Column("first_name")]
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name of the Renter.
    /// </summary>
    [Required]
    [StringLength(30)]
    [Column("last_name")]
    public required string LastName { get; set; }

    /// <summary>
    /// Patronymic (middle name) of the Renter. Optional.
    /// </summary>
    [StringLength(30)]
    [Column("patronymic")]
    public string? Patronymic { get; set; }

    /// <summary>
    /// Contact phone number of the Renter.
    /// </summary>
    [Required]
    [StringLength(20)]
    [Column("phone")]
    public required string Phone { get; set; }
}
