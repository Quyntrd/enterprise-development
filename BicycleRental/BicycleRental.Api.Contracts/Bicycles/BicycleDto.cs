namespace BicycleRental.Api.Contracts.Bicycles;
/// <summary>
/// DTO for GET responses for bicycles
/// </summary>
/// <param name="Id">Identifier of the bicycle</param>
/// <param name="SerialNumber">Serial number</param>
/// <param name="ModelId">Model identifier</param>
/// <param name="Color">Color</param>
public record BicycleDto(
    int Id,
    string? SerialNumber,
    int ModelId,
    string? Color);
