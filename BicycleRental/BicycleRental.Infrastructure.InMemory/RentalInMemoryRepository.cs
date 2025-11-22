using BicycleRental.Domain;
using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for Rental.
/// </summary>
public class RentalInMemoryRepository : IRepository<Rental, int>
{
    private readonly List<Rental> _rentals = new BicycleRentalDataSeed().Rentals;

    /// <inheritdoc/>
    public Task<Rental> Create(Rental entity)
    {
        _rentals.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public Task<bool> Delete(int id)
    {
        var existing = _rentals.FirstOrDefault(r => r.Id == id);
        if (existing != null)
        {
            _rentals.Remove(existing);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public Task<Rental?> Read(int id)
    {
        var result = _rentals.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<IList<Rental>> ReadAll()
    {
        IList<Rental> result = _rentals.ToList();
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<Rental> Update(Rental entity)
    {
        var existing = _rentals.FirstOrDefault(r => r.Id == entity.Id);
        if (existing != null)
            _rentals.Remove(existing);

        _rentals.Add(entity);
        return Task.FromResult(entity);
    }
}
