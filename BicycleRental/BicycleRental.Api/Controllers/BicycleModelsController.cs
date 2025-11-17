using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for bicycle model CRUD operations and related queries.
/// </summary>
/// <param name="service">Application service handling bicycle model use-cases.</param>
/// <param name="logger">Logger instance.</param>
public class BicycleModelsController(IBicycleModelService service, ILogger<BicycleModelsController> logger) : CrudControllerBase<BicycleModelDto, BicycleModelCreateUpdateDto, int>(service, logger)
{
    /// <summary>
    /// Get list of bicycles belonging to a specific bicycle model.
    /// </summary>
    /// <param name="id">Identifier of the bicycle model.</param>
    /// <returns>List of BicycleDto related to the model, or NoContent if none found.</returns>
    [HttpGet("{id}/bicycles")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
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
