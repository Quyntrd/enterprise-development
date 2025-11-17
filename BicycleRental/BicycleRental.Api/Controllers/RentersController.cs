using BicycleRental.Application.Contracts.Renters;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Contracts.Rentals;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for renter (client) CRUD operations and renter-specific queries.
/// </summary>
/// <param name="service">Application service for renters.</param>
/// <param name="rentalService">Application service for rentals.</param>
/// <param name="logger">Logger instance.</param>
public class RentersController(IRenterService service, IRentalService rentalService, ILogger<RentersController> logger)
    : CrudControllerBase<RenterDto, RenterCreateUpdateDto, int>(service, logger)
{
    /// <summary>
    /// Get all rentals made by a specific renter.
    /// </summary>
    /// <param name="id">Identifier of the renter.</param>
    /// <returns>List of RentalDto for the renter, or NoContent if none exist.</returns>
    [HttpGet("{id}/rentals")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public ActionResult<IList<RentalDto>> GetRentals(int id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(GetRentals), GetType().Name, id);
        try
        {
            var res = rentalService.GetByRenterId(id);
            return res != null && res.Count > 0 ? Ok(res) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(GetRentals), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }
}
