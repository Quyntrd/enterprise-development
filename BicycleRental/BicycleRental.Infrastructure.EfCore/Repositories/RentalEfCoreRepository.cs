using BicycleRental.Domain;
using BicycleRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleRental.Infrastructure.EfCore.Repositories;

/// <summary>
/// EF Core repository for Rental entity.
/// </summary>
public class RentalEfCoreRepository(BicycleRentalDbContext context) : IRepository<Rental, int>
{
    private readonly DbSet<Rental> _rentals = context.Rentals;

    /// <inheritdoc/>
    public async Task<Rental> Create(Rental entity)
    {
        var result = await _rentals.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int entityId)
    {
        var entity = await _rentals.FirstOrDefaultAsync(e => e.Id == entityId);
        if (entity == null) return false;
        _rentals.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc/>
    public async Task<Rental?> Read(int entityId) =>
        await _rentals.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entityId);

    /// <inheritdoc/>
    public async Task<IList<Rental>> ReadAll() =>
        await _rentals.AsNoTracking().ToListAsync();

    /// <inheritdoc/>
    public async Task<Rental> Update(Rental entity)
    {
        _rentals.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
