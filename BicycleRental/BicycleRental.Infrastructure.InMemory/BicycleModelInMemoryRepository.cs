using BicycleRental.Domain;
using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;

namespace BicycleRental.Infrastructure.InMemory;

/// <summary>
/// In-memory repository implementation for BicycleModel.
/// </summary>
public class BicycleModelInMemoryRepository : IRepository<BicycleModel, int>
{
    private readonly List<BicycleModel> _models = new BicycleRentalDataSeed().BicycleModels;

    /// <inheritdoc/>
    public Task<BicycleModel> Create(BicycleModel entity)
    {
        _models.Add(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public Task<bool> Delete(int id)
    {
        var existing = _models.FirstOrDefault(m => m.Id == id);
        if (existing != null)
        {
            _models.Remove(existing);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    /// <inheritdoc/>
    public Task<BicycleModel?> Read(int id)
    {
        var result = _models.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<IList<BicycleModel>> ReadAll()
    {
        IList<BicycleModel> result = _models.ToList();
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public Task<BicycleModel> Update(BicycleModel entity)
    {
        var existing = _models.FirstOrDefault(m => m.Id == entity.Id);
        if (existing != null)
            _models.Remove(existing);

        _models.Add(entity);
        return Task.FromResult(entity);
    }
}
