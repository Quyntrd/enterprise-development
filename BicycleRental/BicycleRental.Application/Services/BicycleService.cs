using AutoMapper;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Services;

/// <summary>
/// Application service for bicycles (CRUD + queries by model).
/// </summary>
/// <remarks>
/// Constructor.
/// </remarks>
public class BicycleService(IRepository<Bicycle, int> repo, IMapper mapper) : IBicycleService
{
    private IRepository<Bicycle, int> _repo = repo;
    private IMapper _mapper = mapper;

    /// <inheritdoc/>
    public BicycleDto Create(BicycleCreateUpdateDto dto)
    {
        var entity = _mapper.Map<Bicycle>(dto);
        var all = _repo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(b => b.Id) : 0;
        entity.Id = lastId + 1;
        _repo.Create(entity);
        return _mapper.Map<BicycleDto>(entity);
    }

    /// <inheritdoc/>
    public void Delete(int dtoId) => _repo.Delete(dtoId);

    /// <inheritdoc/>
    public BicycleDto Get(int dtoId) =>
        _mapper.Map<BicycleDto>(_repo.Read(dtoId) ?? throw new KeyNotFoundException("Bicycle not found"));

    /// <inheritdoc/>
    public List<BicycleDto> GetAll() =>
        _mapper.Map<List<BicycleDto>>(_repo.ReadAll());

    /// <inheritdoc/>
    public BicycleDto Update(BicycleCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<Bicycle>(dto);
        upd.Id = dtoId;
        _repo.Update(upd);
        return _mapper.Map<BicycleDto>(upd);
    }

    /// <inheritdoc/>
    public List<BicycleDto> GetByModelId(int modelId)
    {
        var list = _repo.ReadAll().Where(b => b.ModelId == modelId).ToList();
        return _mapper.Map<List<BicycleDto>>(list);
    }
}
