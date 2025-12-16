namespace BicycleRental.Generator.RabbitMq.Host.Services;

/// <summary>
/// Strategy contract that describes how DTO batches are generated
/// and published to the message broker.
/// </summary>
/// <typeparam name="T">DTO type being generated</typeparam>
public interface IGeneratorStrategy<T>
{
    /// <summary>
    /// Generates a batch of DTO objects.
    /// </summary>
    /// <param name="count">Number of items to generate</param>
    /// <returns>Generated DTO list</returns>
    public IList<T> Generate(int count);

    /// <summary>
    /// Publishes a batch of DTOs using the provided producer service.
    /// </summary>
    /// <param name="producer">Producer service</param>
    /// <param name="batch">Batch of DTOs</param>
    public Task PublishAsync(IProducerService producer, IList<T> batch);
}
