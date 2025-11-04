namespace BicycleRental.Api.Contracts.Renters;
/// <summary>
/// DTO for GET responses for renters
/// </summary>
/// <param name="Id">Identifier of the renter</param>
/// <param name="FirstName">First name</param>
/// <param name="LastName">Last name</param>
/// <param name="Patronymic">Patronymic</param>
/// <param name="Phone">Phone</param>
public record RenterDto(
    int Id,
    string? FirstName,
    string? LastName,
    string? Patronymic,
    string? Phone);
