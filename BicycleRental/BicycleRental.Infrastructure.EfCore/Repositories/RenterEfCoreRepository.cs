using BicycleRental.Domain;
using BicycleRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleRental.Infrastructure.EfCore.Repositories;

/// <summary>
/// EF Core repository for Renter entity.
/// </summary>
public class RenterEfCoreRepository(BicycleRentalDbContext context) : IRepository<Renter, int>
{
    private readonly DbSet<Renter> _renters = context.Renters;

    /// <inheritdoc/>
    public async Task<Renter> Create(Renter entity)
    {
        var result = await _renters.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int entityId)
    {
        var entity = await _renters.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) return false;
        _renters.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc/>
    public async Task<Renter?> Read(int entityId) =>
        await _renters.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entityId);

    /// <inheritdoc/>
    public async Task<IList<Renter>> ReadAll() =>
        await _renters.AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public async Task<Renter> Update(Renter entity)
    {
        _renters.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
