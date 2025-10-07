namespace BicycleRental.Domain.Models;

/// <summary>
/// Represents a client who can rent bicycles from the rental point.
/// </summary>
public class Renter
{
    /// <summary>
    /// Unique identifier of the Renter.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// First name of the Renter.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Last name of the Renter.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Patronymic (middle name) of the Renter. Optional.
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    /// Contact phone number of the Renter.
    /// </summary>
    public required string Phone { get; set; }
}
