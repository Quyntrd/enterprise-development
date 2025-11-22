using AutoMapper;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Services;

/// <summary>
/// Application service for rentals (CRUD + queries by bicycle/renter).
/// Price calculations are performed here using the current price in BicycleModel repository.
/// </summary>
public class RentalService(
    IRepository<Rental, int> rentalRepo,
    IRepository<Bicycle, int> bicycleRepo,
    IRepository<BicycleModel, int> modelRepo,
    IMapper mapper) : IRentalService
{
    private IRepository<Rental, int> _rentalRepo = rentalRepo;
    private IRepository<Bicycle, int> _bicycleRepo = bicycleRepo;
    private IRepository<BicycleModel, int> _modelRepo = modelRepo;
    private IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<RentalDto> Create(RentalCreateUpdateDto dto)
    {
        var bicycle = await _bicycleRepo.Read(dto.BicycleId) ?? throw new KeyNotFoundException("Bicycle not found");
        var entity = _mapper.Map<Rental>(dto);
        var all = await _rentalRepo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(r => r.Id) : 0;
        entity.Id = lastId + 1;
        await _rentalRepo.Create(entity);

        return await MapWithPrice(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int dtoId) => await _rentalRepo.Delete(dtoId);

    /// <inheritdoc/>
    public async Task<RentalDto> Get(int dtoId) =>
        await MapWithPrice(await _rentalRepo.Read(dtoId) ?? throw new KeyNotFoundException("Rental not found"));

    /// <inheritdoc/>
    public async Task<List<RentalDto>> GetAll()
    {
        var all = await _rentalRepo.ReadAll();
        var mappedTasks = all.Select(MapWithPrice);
        var results = await Task.WhenAll(mappedTasks);
        return results.ToList();
    }

    /// <inheritdoc/>
    public async Task<RentalDto> Update(RentalCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<Rental>(dto);
        upd.Id = dtoId;
        var updated = await _rentalRepo.Update(upd);
        return await MapWithPrice(updated);
    }

    /// <inheritdoc/>
    public async Task<List<RentalDto>> GetByBicycleId(int bicycleId)
    {
        var list = (await _rentalRepo.ReadAll()).Where(r => r.BicycleId == bicycleId).ToList();
        var mapped = await Task.WhenAll(list.Select(MapWithPrice));
        return mapped.ToList();
    }

    /// <inheritdoc/>
    public async Task<List<RentalDto>> GetByRenterId(int renterId)
    {
        var list = (await _rentalRepo.ReadAll()).Where(r => r.RenterId == renterId).ToList();
        var mapped = await Task.WhenAll(list.Select(MapWithPrice));
        return mapped.ToList();
    }

    /// <summary>
    /// Helper that maps rental and fills price fields using BicycleModel current price.
    /// </summary>
    private async Task<RentalDto> MapWithPrice(Rental r)
    {
        var dto = _mapper.Map<RentalDto>(r);

        var bicycle = await _bicycleRepo.Read(r.BicycleId);
        if (bicycle is null)
        {
            dto = dto with { PricePerHour = 0m, TotalPrice = 0m };
            return dto;
        }

        var model = await _modelRepo.Read(bicycle.ModelId);
        var pricePerHour = model?.PricePerHour ?? 0m;
        var hours = (decimal)r.DurationHours.TotalHours;
        var total = decimal.Round(pricePerHour * hours, 2);

        return dto with { PricePerHour = pricePerHour, TotalPrice = total };
    }
}
