using BicycleRental.Domain;
using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for Bicycle.
/// </summary>
public class BicycleInMemoryRepository : IRepository<Bicycle, int>
{
    private readonly List<Bicycle> _bicycles = new BicycleRentalDataSeed().Bicycles;

    /// <inheritdoc/>
    public Task<Bicycle> Create(Bicycle entity)
    {
        _bicycles.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public Task<bool> Delete(int id)
    {
        var existing = _bicycles.FirstOrDefault(b => b.Id == id);
        if (existing != null)
        {
            _bicycles.Remove(existing);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public Task<Bicycle?> Read(int id)
    {
        var result = _bicycles.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<IList<Bicycle>> ReadAll()
    {
        IList<Bicycle> result = _bicycles.ToList();
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<Bicycle> Update(Bicycle entity)
    {
        var existing = _bicycles.FirstOrDefault(b => b.Id == entity.Id);
        if (existing != null)
            _bicycles.Remove(existing);

        _bicycles.Add(entity);
        return Task.FromResult(entity);
    }
}
