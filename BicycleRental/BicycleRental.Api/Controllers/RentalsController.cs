using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Contracts.Rentals;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Controller for rental CRUD operations.
/// </summary>
/// <param name="service">Application service for rentals.</param>
/// <param name="logger">Logger instance.</param>
public class RentalsController(IRentalService service, ILogger<RentalsController> logger)
    : CrudControllerBase<RentalDto, RentalCreateUpdateDto, int>(service, logger);