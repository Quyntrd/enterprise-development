using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;
using BicycleRental.Domain;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for Rental.
/// </summary>
public class RentalInMemoryRepository : IRepository<Rental, int>
{
    private readonly List<Rental> _rentals;

    /// <summary>
    /// Constructor initializes repository from data seed.
    /// </summary>
    public RentalInMemoryRepository()
    {
        _rentals = new BicycleRentalDataSeed().Rentals;
    }

    /// <inheritdoc/>
    public Rental Create(Rental entity)
    {
        _rentals.Add(entity);
        return entity;
    }

    /// <inheritdoc/>
    public void Delete(int entityId)
    {
        var existing = Read(entityId);
        if (existing != null)
            _rentals.Remove(existing);
    }

    /// <inheritdoc/>
    public Rental? Read(int entityId)
    {
        return _rentals.FirstOrDefault(r => r.Id == entityId);
    }

    /// <inheritdoc/>
    public List<Rental> ReadAll()
    {
        return _rentals.ToList();
    }

    /// <inheritdoc/>
    public void Update(Rental entity)
    {
        Delete(entity.Id);
        Create(entity);
    }
}
