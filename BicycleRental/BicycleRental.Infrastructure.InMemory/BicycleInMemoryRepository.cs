using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;
using BicycleRental.Domain;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for Bicycle.
/// </summary>
public class BicycleInMemoryRepository : IRepository<Bicycle, int>
{
    private readonly List<Bicycle> _bicycles = new BicycleRentalDataSeed().Bicycles;

    /// <inheritdoc/>
    public Bicycle Create(Bicycle entity)
    {
        _bicycles.Add(entity);
        return entity;
    }

    /// <inheritdoc/>
    public void Delete(int entityId)
    {
        var existing = Read(entityId);
        if (existing != null)
            _bicycles.Remove(existing);
    }

    /// <inheritdoc/>
    public Bicycle? Read(int entityId)
    {
        return _bicycles.FirstOrDefault(b => b.Id == entityId);
    }

    /// <inheritdoc/>
    public List<Bicycle> ReadAll()
    {
        return _bicycles.ToList();
    }

    /// <inheritdoc/>
    public void Update(Bicycle entity)
    {
        Delete(entity.Id);
        Create(entity);
    }
}
