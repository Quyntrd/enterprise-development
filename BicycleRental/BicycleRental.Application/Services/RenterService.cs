using AutoMapper;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Services;

/// <summary>
/// Application service for renters (clients).
/// </summary>
public class RenterService(IRepository<Renter, int> repo, IRepository<Rental, int> rentalRepo, IMapper mapper) : IRenterService
{
    private IRepository<Renter, int> _repo = repo;
    private IRepository<Rental, int> _rentalRepo = rentalRepo;
    private IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<RenterDto> Create(RenterCreateUpdateDto dto)
    {
        var entity = _mapper.Map<Renter>(dto);
        var all = await _repo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(r => r.Id) : 0;
        entity.Id = lastId + 1;
        await _repo.Create(entity);
        return _mapper.Map<RenterDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int dtoId) => await _repo.Delete(dtoId);

    /// <inheritdoc/>
    public async Task<RenterDto> Get(int dtoId)
    {
        var entity = await _repo.Read(dtoId) ?? throw new KeyNotFoundException("Renter not found");
        return _mapper.Map<RenterDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<List<RenterDto>> GetAll()
    {
        var list = await _repo.ReadAll();
        return _mapper.Map<List<RenterDto>>(list);
    }

    /// <inheritdoc/>
    public async Task<RenterDto> Update(RenterCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<Renter>(dto);
        upd.Id = dtoId;
        var updated = await _repo.Update(upd);
        return _mapper.Map<RenterDto>(updated);
    }

    /// <inheritdoc/>
    public async Task<List<RentalDto>> GetRentals(int dtoId)
    {
        var rentals = (await _rentalRepo.ReadAll()).Where(r => r.RenterId == dtoId).ToList();
        return _mapper.Map<List<RentalDto>>(rentals);
    }
}
