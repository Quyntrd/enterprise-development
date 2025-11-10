using BicycleRental.Api.Contracts.Bicycles;
using BicycleRental.Api.Contracts.Contracts;
using BicycleRental.Api.Contracts.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for bicycle CRUD operations and bicycle-specific queries.
/// </summary>
/// <param name="service">Application service for bicycles.</param>
/// <param name="rentalService">Application service for rentals.</param>
/// <param name="logger">Logger instance.</param>
public class BicyclesController(IBicycleService service, IRentalService rentalService, ILogger<BicyclesController> logger)
    : CrudControllerBase<BicycleDto, BicycleCreateUpdateDto, int>(service, logger)
{
    /// <summary>
    /// Get all rentals for a given bicycle.
    /// </summary>
    /// <param name="id">Identifier of the bicycle.</param>
    /// <returns>List of RentalDto for the bicycle, or NoContent if none exist.</returns>
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
