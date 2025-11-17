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
/// <remarks>
/// Constructor.
/// </remarks>
public class RenterService(IRepository<Renter, int> repo, IRepository<Rental, int> rentalRepo, IMapper mapper) : IRenterService
{
    private IRepository<Renter, int> _repo = repo;
    private IRepository<Rental, int> _rentalRepo = rentalRepo;
    private IMapper _mapper = mapper;

    /// <inheritdoc/>
    public RenterDto Create(RenterCreateUpdateDto dto)
    {
        var entity = _mapper.Map<Renter>(dto);
        var all = _repo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(r => r.Id) : 0;
        entity.Id = lastId + 1;
        _repo.Create(entity);
        return _mapper.Map<RenterDto>(entity);
    }

    /// <inheritdoc/>
    public void Delete(int dtoId) => _repo.Delete(dtoId);

    /// <inheritdoc/>
    public RenterDto Get(int dtoId) =>
        _mapper.Map<RenterDto>(_repo.Read(dtoId) ?? throw new KeyNotFoundException("Renter not found"));

    /// <inheritdoc/>
    public List<RenterDto> GetAll() =>
        _mapper.Map<List<RenterDto>>(_repo.ReadAll());

    /// <inheritdoc/>
    public RenterDto Update(RenterCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<Renter>(dto);
        upd.Id = dtoId;
        _repo.Update(upd);
        return _mapper.Map<RenterDto>(upd);
    }

    /// <inheritdoc/>
    public List<RentalDto> GetRentals(int dtoId)
    {
        var rentals = _rentalRepo.ReadAll().Where(r => r.RenterId == dtoId).ToList();
        return _mapper.Map<List<RentalDto>>(rentals);
    }
}
