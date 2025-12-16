using BicycleRental.Application.Contracts.Rentals;
using Bogus;

namespace BicycleRental.Generator.RabbitMq.Host.Generators;

/// <summary>
/// Generates random RentalCreateUpdateDto objects for testing.
/// </summary>
public static class RentalGenerator
{
    /// <summary>
    /// Generates a collection of random rentals using predefined faker rules
    /// </summary>
    /// <param name="count">Number of rentals to generate</param>
    /// <returns>List of randomly generated <see cref="RentalCreateUpdateDto"/> objects</returns>
    public static List<RentalCreateUpdateDto> GenerateRentals(int count)
    {
        var faker = new Faker<RentalCreateUpdateDto>()
            .WithRecord()
            .RuleFor(d => d.BicycleId, f => f.Random.Int(1, 20))
            .RuleFor(d => d.RenterId, f => f.Random.Int(1, 20))
            .RuleFor(d => d.StartAt, f => f.Date.RecentOffset(days: 60).UtcDateTime)
            .RuleFor(d => d.DurationHours, f => TimeSpan.FromHours(f.Random.Double(0.25, 48.0)));

        return faker.Generate(count);
    }
}