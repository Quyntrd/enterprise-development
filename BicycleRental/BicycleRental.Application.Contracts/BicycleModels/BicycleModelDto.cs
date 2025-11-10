namespace BicycleRental.Application.Contracts.BicycleModels;
/// <summary>
/// DTO for GET responses for bicycle models
/// </summary>
/// <param name="Id">Identifier of the bicycle model</param>
/// <param name="Name">Model name</param>
/// <param name="Type">Type of bicycle as string</param>
/// <param name="WheelSizeInInches">Wheel size in inches</param>
/// <param name="MaxPassengerWeightKg">Max passenger weight in kg</param>
/// <param name="WeightKg">Weight in kg</param>
/// <param name="BrakeType">Brake type</param>
/// <param name="ModelYear">Model year</param>
/// <param name="PricePerHour">Rental price per hour</param>
public record BicycleModelDto(
    int Id,
    string? Name,
    string? Type,
    double? WheelSizeInInches,
    double? MaxPassengerWeightKg,
    double? WeightKg,
    string? BrakeType,
    int? ModelYear,
    decimal PricePerHour);
