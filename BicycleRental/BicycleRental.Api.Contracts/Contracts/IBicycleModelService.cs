using BicycleRental.Api.Contracts.Bicycles;

namespace BicycleRental.Api.Contracts.Contracts;

/// <summary>
/// Application service for bicycle models.
/// </summary>
public interface IBicycleModelService : IApplicationService<
    BicycleModels.BicycleModelDto,
    BicycleModels.BicycleModelCreateUpdateDto,
    int>
{
    /// <summary>
    /// Gets collection of bicycles related to the bicycle model.
    /// </summary>
    /// <param name="dtoId">Model identifier</param>
    /// <returns>List of bicycles</returns>
    List<BicycleDto> GetBicycles(int dtoId);
}
