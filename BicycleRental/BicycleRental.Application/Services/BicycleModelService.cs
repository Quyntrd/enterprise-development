using AutoMapper;
using BicycleRental.Api.Contracts.BicycleModels;
using BicycleRental.Api.Contracts.Bicycles;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Services;

/// <summary>
/// Application service for bicycle models (CRUD + related bicycles).
/// </summary>
/// <remarks>
/// Depends on a repository for BicycleModel and a repository for Bicycle to return related entities.
/// </remarks>
public class BicycleModelService : BicycleRental.Api.Contracts.Contracts.IBicycleModelService
{
    private readonly IRepository<BicycleModel, int> _modelRepo;
    private readonly IRepository<Bicycle, int> _bicycleRepo;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor.
    /// </summary>
    public BicycleModelService(
        IRepository<BicycleModel, int> modelRepo,
        IRepository<Bicycle, int> bicycleRepo,
        IMapper mapper)
    {
        _modelRepo = modelRepo;
        _bicycleRepo = bicycleRepo;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public BicycleModelDto Create(BicycleModelCreateUpdateDto dto)
    {
        var entity = _mapper.Map<BicycleModel>(dto);
        var all = _modelRepo.ReadAll();
        var lastId = all.Count != 0 ? all.Max(m => m.Id) : 0;
        entity.Id = lastId + 1;
        _modelRepo.Create(entity);
        return _mapper.Map<BicycleModelDto>(entity);
    }

    /// <inheritdoc/>
    public void Delete(int dtoId) => _modelRepo.Delete(dtoId);

    /// <inheritdoc/>
    public BicycleModelDto Get(int dtoId) =>
        _mapper.Map<BicycleModelDto>(_modelRepo.Read(dtoId) ?? throw new KeyNotFoundException("BicycleModel not found"));

    /// <inheritdoc/>
    public List<BicycleModelDto> GetAll() =>
        _mapper.Map<List<BicycleModelDto>>(_modelRepo.ReadAll());

    /// <inheritdoc/>
    public BicycleModelDto Update(BicycleModelCreateUpdateDto dto, int dtoId)
    {
        var upd = _mapper.Map<BicycleModel>(dto);
        upd.Id = dtoId;
        _modelRepo.Update(upd);
        return _mapper.Map<BicycleModelDto>(upd);
    }

    /// <inheritdoc/>
    public List<BicycleDto> GetBicycles(int dtoId)
    {
        var bikes = _bicycleRepo.ReadAll().Where(b => b.ModelId == dtoId).ToList();
        return _mapper.Map<List<BicycleDto>>(bikes);
    }
}
