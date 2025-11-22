using BicycleRental.Domain;
using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for Renter.
/// </summary>
public class RenterInMemoryRepository : IRepository<Renter, int>
{
    private readonly List<Renter> _renters = new BicycleRentalDataSeed().Renters;

    /// <inheritdoc/>
    public Task<Renter> Create(Renter entity)
    {
        _renters.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public Task<bool> Delete(int id)
    {
        var existing = _renters.FirstOrDefault(r => r.Id == id);
        if (existing != null)
        {
            _renters.Remove(existing);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public Task<Renter?> Read(int id)
    {
        var result = _renters.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<IList<Renter>> ReadAll()
    {
        IList<Renter> result = _renters.ToList();
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<Renter> Update(Renter entity)
    {
        var existing = _renters.FirstOrDefault(r => r.Id == entity.Id);
        if (existing != null)
            _renters.Remove(existing);

        _renters.Add(entity);
        return Task.FromResult(entity);
    }
}
