using BicycleRental.Domain;
using BicycleRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleRental.Infrastructure.EfCore.Repositories;

/// <summary>
/// EF Core repository for Bicycle entity.
/// </summary>
public class BicycleEfCoreRepository(BicycleRentalDbContext context) : IRepository<Bicycle, int>
{
    private readonly DbSet<Bicycle> _bicycles = context.Bicycles;

    public async Task<Bicycle> Create(Bicycle entity)
    {
        var result = await _bicycles.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int entityId)
    {
        var entity = await _bicycles.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) return false;
        _bicycles.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Bicycle?> Read(int entityId) =>
        await _bicycles.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entityId);

    public async Task<IList<Bicycle>> ReadAll() =>
        await _bicycles.AsNoTracking().ToListAsync();

    public async Task<Bicycle> Update(Bicycle entity)
    {
        _bicycles.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
