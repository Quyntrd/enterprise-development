namespace BikeRental.Domain.Models;

/// <summary>
///
/// </summary>
public class Renter
{
    /// <summary>
    ///
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Patronymic { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required string Phone { get; set; }
}
