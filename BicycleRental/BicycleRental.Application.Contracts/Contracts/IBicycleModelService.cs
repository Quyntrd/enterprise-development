using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.BicycleModels;

namespace BicycleRental.Application.Contracts.Contracts;

/// <summary>
/// Application service for bicycle models.
/// </summary>
public interface IBicycleModelService : IApplicationService<
    BicycleModelDto,
    BicycleModelCreateUpdateDto,
    int>
{
    /// <summary>
    /// Gets collection of bicycles related to the bicycle model.
    /// </summary>
    /// <param name="dtoId">Model identifier</param>
    /// <returns>List of bicycles</returns>
    public List<BicycleDto> GetBicycles(int dtoId);
}
