using BicycleRental.Api.Contracts.BicycleModels;
using BicycleRental.Api.Contracts.Bicycles;
using BicycleRental.Api.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for bicycle models
/// </summary>
/// <param name="service">Application service for bicycle models</param>
/// <param name="logger">Logger</param>
public class BicycleModelsController(IBicycleModelService service, ILogger<BicycleModelsController> logger) : CrudControllerBase<BicycleModelDto, BicycleModelCreateUpdateDto, int>(service, logger)
{
    /// <summary>
    /// Get bicycles by model id
    /// </summary>
    [HttpGet("{id}/bicycles")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    public ActionResult<IList<BicycleDto>> GetBicycles(int id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(GetBicycles), GetType().Name, id);
        try
        {
            var res = service.GetBicycles(id);
            return res != null && res.Count > 0 ? Ok(res) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(GetBicycles), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }
}
