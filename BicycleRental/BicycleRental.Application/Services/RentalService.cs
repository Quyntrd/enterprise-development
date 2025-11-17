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
/// <remarks>
/// Constructor.
/// </remarks>
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
    public RentalDto Create(RentalCreateUpdateDto dto)
    {
        var bicycle = _bicycleRepo.Read(dto.BicycleId) ?? throw new KeyNotFoundException("Bicycle not found");
        var entity = _mapper.Map<Rental>(dto);
        var all = _rentalRepo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(r => r.Id) : 0;
        entity.Id = lastId + 1;
        _rentalRepo.Create(entity);

        return MapWithPrice(entity);
    }

    /// <inheritdoc/>
    public void Delete(int dtoId) => _rentalRepo.Delete(dtoId);

    /// <inheritdoc/>
    public RentalDto Get(int dtoId) =>
        MapWithPrice(_rentalRepo.Read(dtoId) ?? throw new KeyNotFoundException("Rental not found"));

    /// <inheritdoc/>
    public List<RentalDto> GetAll() =>
        _rentalRepo.ReadAll().Select(MapWithPrice).ToList();

    /// <inheritdoc/>
    public RentalDto Update(RentalCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<Rental>(dto);
        upd.Id = dtoId;
        _rentalRepo.Update(upd);
        return MapWithPrice(upd);
    }

    /// <inheritdoc/>
    public List<RentalDto> GetByBicycleId(int bicycleId) =>
        _rentalRepo.ReadAll().Where(r => r.BicycleId == bicycleId).Select(MapWithPrice).ToList();

    /// <inheritdoc/>
    public List<RentalDto> GetByRenterId(int renterId) =>
        _rentalRepo.ReadAll().Where(r => r.RenterId == renterId).Select(MapWithPrice).ToList();

    /// <summary>
    /// Helper that maps rental and fills price fields using BicycleModel current price.
    /// </summary>
    private RentalDto MapWithPrice(Rental r)
    {
        var dto = _mapper.Map<RentalDto>(r);

        var bicycle = _bicycleRepo.Read(r.BicycleId);
        if (bicycle is null)
        {
            dto = dto with { PricePerHour = 0m, TotalPrice = 0m };
            return dto;
        }

        var model = _modelRepo.Read(bicycle.ModelId);
        var pricePerHour = model?.PricePerHour ?? 0m;
        var hours = (decimal)r.DurationHours.TotalHours;
        var total = decimal.Round(pricePerHour * hours, 2);

        return dto with { PricePerHour = pricePerHour, TotalPrice = total };
    }
}
