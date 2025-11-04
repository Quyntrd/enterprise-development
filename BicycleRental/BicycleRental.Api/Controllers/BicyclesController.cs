using BicycleRental.Api.Contracts.Bicycles;
using BicycleRental.Api.Contracts.Contracts;
using BicycleRental.Api.Contracts.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for bicycles
/// </summary>
public class BicyclesController(IBicycleService service, IRentalService rentalService, ILogger<BicyclesController> logger)
    : CrudControllerBase<BicycleDto, BicycleCreateUpdateDto, int>(service, logger)
{
    [HttpGet("{id}/rentals")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    public ActionResult<IList<RentalDto>> GetRentals(int id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(GetRentals), GetType().Name, id);
        try
        {
            var res = rentalService.GetByBicycleId(id);
            return res != null && res.Count > 0 ? Ok(res) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(GetRentals), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }
}
