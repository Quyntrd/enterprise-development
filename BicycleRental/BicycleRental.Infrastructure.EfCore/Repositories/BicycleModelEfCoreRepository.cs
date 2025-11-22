using BicycleRental.Domain;
using BicycleRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleRental.Infrastructure.EfCore.Repositories;

/// <summary>
/// EF Core repository for BicycleModel entity.
/// </summary>
public class BicycleModelEfCoreRepository(BicycleRentalDbContext context) : IRepository<BicycleModel, int>
{
    private readonly DbSet<BicycleModel> _bicyclemodels = context.BicycleModels;

    public async Task<BicycleModel> Create(BicycleModel entity)
    {
        var result = await _bicyclemodels.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await _bicyclemodels.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) return false;
        _bicyclemodels.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<BicycleModel?> Read(int entityId) =>
        await _bicyclemodels.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<BicycleModel>> ReadAll() =>
        await _bicyclemodels.AsNoTracking().ToListAsync();

    public async Task<BicycleModel> Update(BicycleModel entity)
    {
        _bicyclemodels.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
