using Bogus;
using System.Runtime.CompilerServices;

namespace BicycleRental.Generator.RabbitMq.Host.Generators;

/// <summary>
/// Extension to configure Faker to create instances without invoking constructors.
/// Useful for DTOs with required properties.
/// </summary>
public static class GeneratorExtensions
{
    /// <summary>
    /// Configures the faker to instantiate objects without calling their constructors
    /// Useful when generating DTOs with required properties
    /// </summary>
    /// <typeparam name="T">Type of the object being generated</typeparam>
    /// <param name="faker">The faker instance to configure</param>
    /// <returns>Configured <see cref="Faker{T}"/> instance</returns>
    public static Faker<T> WithRecord<T>(this Faker<T> faker) where T : class =>
        faker.CustomInstantiator(_ => (T)RuntimeHelpers.GetUninitializedObject(typeof(T)));
}