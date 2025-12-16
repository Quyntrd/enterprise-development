using BicycleRental.Application.Contracts.Renters;
using Bogus;

namespace BicycleRental.Generator.RabbitMq.Host.Generators;

/// <summary>
/// Generates random RenterCreateUpdateDto objects for testing.
/// </summary>
public static class RenterGenerator
{
    /// <summary>
    /// Generates a collection of random renters using predefined faker rules
    /// </summary>
    /// <param name="count">Number of renters to generate</param>
    /// <returns>List of randomly generated <see cref="RenterCreateUpdateDto"/> objects</returns>
    public static List<RenterCreateUpdateDto> GenerateRenters(int count) =>
        new Faker<RenterCreateUpdateDto>()
            .WithRecord()
            .RuleFor(d => d.FirstName, f => f.Person.FirstName)
            .RuleFor(d => d.LastName, f => f.Person.LastName)
            .RuleFor(d => d.Phone, f => f.Phone.PhoneNumber("+7-9##-###-####"))
            .Generate(count);
}