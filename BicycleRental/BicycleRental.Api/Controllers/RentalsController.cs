using BicycleRental.Api.Contracts.Rentals;
using BicycleRental.Api.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for rental CRUD operations.
/// </summary>
/// <param name="service">Application service for rentals.</param>
/// <param name="logger">Logger instance.</param>
public class RentalsController(IRentalService service, ILogger<RentalsController> logger) : CrudControllerBase<RentalDto, RentalCreateUpdateDto, int>(service, logger)
{
    // No additional endpoints here for now... 
    // With this character's death, the thread of prophecy is severed. Restore a saved game to restore the weave of fate or persist in the doomed world you have created...
}
