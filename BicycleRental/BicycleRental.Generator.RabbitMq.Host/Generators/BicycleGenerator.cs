using BicycleRental.Application.Contracts.Bicycles;
using Bogus;

namespace BicycleRental.Generator.RabbitMq.Host.Generators;

/// <summary>
/// Generates random BicycleCreateUpdateDto objects for testing.
/// </summary>
public static class BicycleGenerator
{
    private static readonly string[] _colors = ["Red", "Black", "Green", "Blue", "White", "Gray", "Yellow", "Orange", "Turquoise", "Brown"];

    /// <summary>
    /// Generates a collection of random bicycles using predefined faker rules
    /// </summary>
    /// <param name="count">Number of bicycles to generate</param>
    /// <returns>List of randomly generated <see cref="BicycleCreateUpdateDto"/> objects</returns>
    public static List<BicycleCreateUpdateDto> GenerateBicycles(int count) =>
        new Faker<BicycleCreateUpdateDto>()
            .WithRecord()
            .RuleFor(d => d.SerialNumber, f => $"SN-{f.Random.Replace("####")}-{f.Random.AlphaNumeric(3).ToUpper()}")
            .RuleFor(d => d.ModelId, f => f.Random.Int(1, 12))
            .RuleFor(d => d.Color, f => f.PickRandom(_colors))
            .Generate(count);
}