using BicycleRental.Application.Contracts.Bicycles;

namespace BicycleRental.Application.Contracts.Contracts;

/// <summary>
/// Application service for bicycles.
/// </summary>
public interface IBicycleService : IApplicationService<
    Bicycles.BicycleDto,
    BicycleCreateUpdateDto,
    int>
{
    /// <summary>
    /// Returns list of bicycles by model id.
    /// </summary>
    /// <param name="modelId">Model identifier</param>
    /// <returns>List of BicycleDto</returns>
    public List<Bicycles.BicycleDto> GetByModelId(int modelId);
}
