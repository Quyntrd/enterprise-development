using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;
using BicycleRental.Domain;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for Renter.
/// </summary>
public class RenterInMemoryRepository : IRepository<Renter, int>
{
    private readonly List<Renter> _renters;

    /// <summary>
    /// Constructor initializes repository from data seed.
    /// </summary>
    public RenterInMemoryRepository()
    {
        _renters = new BicycleRentalDataSeed().Renters;
    }

    /// <inheritdoc/>
    public Renter Create(Renter entity)
    {
        _renters.Add(entity);
        return entity;
    }

    /// <inheritdoc/>
    public void Delete(int entityId)
    {
        var existing = Read(entityId);
        if (existing != null)
            _renters.Remove(existing);
    }

    /// <inheritdoc/>
    public Renter Read(int entityId)
    {
        return _renters.First(r => r.Id == entityId);
    }

    /// <inheritdoc/>
    public List<Renter> ReadAll()
    {
        return _renters.ToList();
    }

    /// <inheritdoc/>
    public void Update(Renter entity)
    {
        Delete(entity.Id);
        Create(entity);
    }
}
