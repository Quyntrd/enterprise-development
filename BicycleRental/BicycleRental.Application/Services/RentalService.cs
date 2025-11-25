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
        var results = new List<RentalDto>(all.Count);
        foreach (var r in all)
        {
            results.Add(await MapWithPrice(r));
        }
        return results;
    }

    /// <inheritdoc/>
    public async Task<RentalDto> Update(RentalCreateUpdateDto dto, int dtoId)
    {
        var existing = await _rentalRepo.Read(dtoId)
            ?? throw new KeyNotFoundException("Rental not found");

        var bicycle = await _bicycleRepo.Read(dto.BicycleId)
            ?? throw new KeyNotFoundException("Bicycle not found");

        existing.BicycleId = dto.BicycleId;
        existing.RenterId = dto.RenterId;
        existing.StartAt = dto.StartAt;
        existing.DurationHours = dto.DurationHours;

        var updated = await _rentalRepo.Update(existing);

        return await MapWithPrice(updated);
    }

    /// <inheritdoc/>
    public async Task<List<RentalDto>> GetByBicycleId(int bicycleId)
    {
        var list = (await _rentalRepo.ReadAll()).Where(r => r.BicycleId == bicycleId).ToList();
        var results = new List<RentalDto>(list.Count);
        foreach (var r in list)
        {
            results.Add(await MapWithPrice(r));
        }
        return results;
    }

    /// <inheritdoc/>
    public async Task<List<RentalDto>> GetByRenterId(int renterId)
    {
        var list = (await _rentalRepo.ReadAll()).Where(r => r.RenterId == renterId).ToList();
        var results = new List<RentalDto>(list.Count);
        foreach (var r in list)
        {
            results.Add(await MapWithPrice(r));
        }
        return results;
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