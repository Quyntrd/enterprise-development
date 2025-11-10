namespace BicycleRental.Application.Contracts.Bicycles;
/// <summary>
/// DTO for POST/PUT requests for bicycles
/// </summary>
/// <param name="SerialNumber">Serial number (e.g. "SN-1001")</param>
/// <param name="ModelId">Identifier of the bicycle model</param>
/// <param name="Color">Color description</param>
public record BicycleCreateUpdateDto(
    string? SerialNumber,
    int ModelId,
    string? Color);
