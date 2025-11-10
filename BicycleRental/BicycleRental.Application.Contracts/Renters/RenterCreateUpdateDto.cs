namespace BicycleRental.Application.Contracts.Renters;
/// <summary>
/// DTO for POST/PUT requests for renters (clients)
/// </summary>
/// <param name="FirstName">First name</param>
/// <param name="LastName">Last name</param>
/// <param name="Patronymic">Patronymic/middle name</param>
/// <param name="Phone">Contact phone</param>
public record RenterCreateUpdateDto(
    string? FirstName,
    string? LastName,
    string? Patronymic,
    string? Phone);
