using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;
using BicycleRental.Domain;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for BicycleModel.
/// </summary>
public class BicycleModelInMemoryRepository : IRepository<BicycleModel, int>
{
    private readonly List<BicycleModel> _models;

    /// <summary>
    /// Constructor initializes repository from data seed.
    /// </summary>
    public BicycleModelInMemoryRepository()
    {
        _models = new BicycleRentalDataSeed().BicycleModels;
    }

    /// <inheritdoc/>
    public BicycleModel Create(BicycleModel entity)
    {
        _models.Add(entity);
        return entity;
    }

    /// <inheritdoc/>
    public void Delete(int entityId)
    {
        var existing = Read(entityId);
        if (existing != null)
            _models.Remove(existing);
    }

    /// <inheritdoc/>
    public BicycleModel? Read(int entityId)
    {
        return _models.FirstOrDefault(m => m.Id == entityId);
    }

    /// <inheritdoc/>
    public List<BicycleModel> ReadAll()
    {
        return _models.ToList();
    }

    /// <inheritdoc/>
    public void Update(BicycleModel entity)
    {
        Delete(entity.Id);
        Create(entity);
    }
}
