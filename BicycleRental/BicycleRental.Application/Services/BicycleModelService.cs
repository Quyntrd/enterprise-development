using AutoMapper;
using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Services;

/// <summary>
/// Application service for bicycle models (CRUD + related bicycles).
/// </summary>
/// <remarks>
/// Depends on a repository for BicycleModel and a repository for Bicycle to return related entities.
/// </remarks>
public class BicycleModelService(
    IRepository<BicycleModel, int> modelRepo,
    IRepository<Bicycle, int> bicycleRepo,
    IMapper mapper) : IBicycleModelService
{
    private IRepository<BicycleModel, int> _modelRepo = modelRepo;
    private IRepository<Bicycle, int> _bicycleRepo = bicycleRepo;
    private IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<BicycleModelDto> Create(BicycleModelCreateUpdateDto dto)
    {
        var entity = _mapper.Map<BicycleModel>(dto);
        var all = await _modelRepo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(m => m.Id) : 0;
        entity.Id = lastId + 1;
        await _modelRepo.Create(entity);
        return _mapper.Map<BicycleModelDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> Delete(int dtoId) => await _modelRepo.Delete(dtoId);

    /// <inheritdoc/>
    public async Task<BicycleModelDto> Get(int dtoId)
    {
        var entity = await _modelRepo.Read(dtoId)
            ?? throw new KeyNotFoundException("BicycleModel not found");
        return _mapper.Map<BicycleModelDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<List<BicycleModelDto>> GetAll()
    {
        var list = await _modelRepo.ReadAll();
        return _mapper.Map<List<BicycleModelDto>>(list);
    }

    /// <inheritdoc/>
    public async Task<BicycleModelDto> Update(BicycleModelCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<BicycleModel>(dto);
        upd.Id = dtoId;
        var updated = await _modelRepo.Update(upd);
        return _mapper.Map<BicycleModelDto>(updated);
    }

    /// <inheritdoc/>
    public async Task<List<BicycleDto>> GetBicycles(int dtoId)
    {
        var bikes = (await _bicycleRepo.ReadAll()).Where(b => b.ModelId == dtoId).ToList();
        return _mapper.Map<List<BicycleDto>>(bikes);
    }
}
