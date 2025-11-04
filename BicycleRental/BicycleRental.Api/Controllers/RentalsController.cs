using BicycleRental.Api.Contracts.Rentals;
using BicycleRental.Api.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for rentals
/// </summary>
public class RentalsController(IRentalService service, ILogger<RentalsController> logger) : CrudControllerBase<RentalDto, RentalCreateUpdateDto, int>(service, logger)
{
    // No additional endpoints here for now
}
