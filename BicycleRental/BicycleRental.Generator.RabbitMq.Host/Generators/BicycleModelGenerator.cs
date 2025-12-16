using BicycleRental.Application.Contracts.BicycleModels;
using Bogus;

namespace BicycleRental.Generator.RabbitMq.Host.Generators;

/// <summary>
/// Generates random BicycleModelCreateUpdateDto objects for testing.
/// </summary>
public static class BicycleModelGenerator
{
    private static readonly string[] _brakeTypes = ["Disc", "V-Brake", "Caliper", "Coaster"];

    /// <summary>
    /// Generates a collection of random bicycle models using predefined faker rules
    /// </summary>
    /// <param name="count">Number of bicycle models to generate</param>
    /// <returns>List of randomly generated <see cref="BicycleModelCreateUpdateDto"/> objects</returns>
    public static List<BicycleModelCreateUpdateDto> GenerateModels(int count) =>
        new Faker<BicycleModelCreateUpdateDto>()
            .WithRecord()
            .RuleFor(d => d.Name, f => $"{f.Commerce.ProductName()} {f.Random.Int(100, 999)}")
            .RuleFor(d => d.Type, f => f.Random.Int(1, 5))
            .RuleFor(d => d.WheelSizeInInches, f => f.PickRandom(new double?[] { 26, 27, 28 }))
            .RuleFor(d => d.MaxPassengerWeightKg, f => f.Random.Double(80, 150))
            .RuleFor(d => d.WeightKg, f => f.Random.Double(6.0, 25.0))
            .RuleFor(d => d.BrakeType, f => f.PickRandom(_brakeTypes))
            .RuleFor(d => d.ModelYear, f => f.Random.Int(2015, DateTime.UtcNow.Year))
            .RuleFor(d => d.PricePerHour, f => decimal.Round((decimal)f.Random.Double(1.5, 15.0), 2))
            .Generate(count);
}