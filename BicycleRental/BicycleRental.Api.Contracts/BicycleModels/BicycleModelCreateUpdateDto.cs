namespace BicycleRental.Api.Contracts.BicycleModels;
/// <summary>
/// DTO for POST/PUT requests for bicycle models
/// </summary>
/// <param name="Name">Model name (e.g. "SportPro 1000")</param>
/// <param name="Type">Bicycle type as string (e.g. "Mountain", "City")</param>
/// <param name="WheelSizeInInches">Wheel size in inches</param>
/// <param name="MaxPassengerWeightKg">Maximum passenger weight in kilograms</param>
/// <param name="WeightKg">Weight of the model in kilograms</param>
/// <param name="BrakeType">Brake type (e.g. "Disc")</param>
/// <param name="ModelYear">Model year</param>
/// <param name="PricePerHour">Rental price per hour</param>
public record BicycleModelCreateUpdateDto(
    string? Name,
    string? Type,
    double? WheelSizeInInches,
    double? MaxPassengerWeightKg,
    double? WeightKg,
    string? BrakeType,
    int? ModelYear,
    decimal? PricePerHour);
