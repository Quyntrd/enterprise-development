using AutoMapper;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Services;

/// <summary>
/// Application service for bicycles (CRUD + queries by model).
/// </summary>
public class BicycleService(IRepository<Bicycle, int> repo, IMapper mapper) : IBicycleService
{
    private IRepository<Bicycle, int> _repo = repo;
    private IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<BicycleDto> Create(BicycleCreateUpdateDto dto)
    {
        var entity = _mapper.Map<Bicycle>(dto);
        var all = await _repo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(b => b.Id) : 0;
        entity.Id = lastId + 1;
        await _repo.Create(entity);
        return _mapper.Map<BicycleDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int dtoId) => await _repo.Delete(dtoId);

    /// <inheritdoc/>
    public async Task<BicycleDto> Get(int dtoId)
    {
        var entity = await _repo.Read(dtoId)
            ?? throw new KeyNotFoundException("Bicycle not found");
        return _mapper.Map<BicycleDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<List<BicycleDto>> GetAll()
    {
        var list = await _repo.ReadAll();
        return _mapper.Map<List<BicycleDto>>(list);
    }

    /// <inheritdoc/>
    public async Task<BicycleDto> Update(BicycleCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<Bicycle>(dto);
        upd.Id = dtoId;
        var updated = await _repo.Update(upd);
        return _mapper.Map<BicycleDto>(updated);
    }

    /// <inheritdoc/>
    public async Task<List<BicycleDto>> GetByModelId(int modelId)
    {
        var list = (await _repo.ReadAll()).Where(b => b.ModelId == modelId).ToList();
        return _mapper.Map<List<BicycleDto>>(list);
    }
}
