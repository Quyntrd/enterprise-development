using BicycleRental.Api.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Generic base controller providing CRUD endpoints.
/// </summary>
/// <typeparam name="TDto">DTO used for GET responses</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO used for POST/PUT</typeparam>
/// <typeparam name="TKey">Identifier type</typeparam>
[Route("api/[controller]")]
[ApiController]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TKey>(IApplicationService<TDto, TCreateUpdateDto, TKey> appService,
    ILogger<CrudControllerBase<TDto, TCreateUpdateDto, TKey>> logger) : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(500)]
    public ActionResult<TDto> Create(TCreateUpdateDto newDto)
    {
        logger.LogInformation("{method} method of {controller} is called with {@dto} parameter", nameof(Create), GetType().Name, newDto);
        try
        {
            var res = appService.Create(newDto);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Create), GetType().Name);

            if (res == null)
            {
                // If service unexpectedly returned null, fallback to 204/500 as you prefer.
                return StatusCode(500, "Created resource is null.");
            }

            // Try to get Id property from returned DTO via reflection.
            // This assumes DTO contains a property named "Id".
            var idProp = res.GetType().GetProperty("Id");
            if (idProp != null)
            {
                var idValue = idProp.GetValue(res);
                if (idValue != null)
                {
                    // CreatedAtAction will build Location header using route to Get action and id value.
                    return CreatedAtAction(nameof(Get), new { id = idValue }, res);
                }
            }

            // Fallback: DTO does not have Id or Id is null — return Created without route-location.
            return Created(string.Empty, res);
        }
        catch (ArgumentException argEx)
        {
            // Validation errors from mapping/parsing -> 400 Bad Request
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(Create), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Create), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public ActionResult<TDto> Edit(TKey id, TCreateUpdateDto newDto)
    {
        logger.LogInformation("{method} called on {controller} with id={id} and payload {@dto}", nameof(Edit), GetType().Name, id, newDto);
        try
        {
            var res = appService.Update(newDto, id);
            logger.LogInformation("{method} executed successfully on {controller}", nameof(Edit), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(Edit), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public IActionResult Delete(TKey id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(Delete), GetType().Name, id);
        try
        {
            appService.Delete(id);
            logger.LogInformation("{method} executed successfully on {controller}", nameof(Delete), GetType().Name);
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(Delete), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public ActionResult<IList<TDto>> GetAll()
    {
        logger.LogInformation("{method} called on {controller}", nameof(GetAll), GetType().Name);
        try
        {
            var res = appService.GetAll();
            logger.LogInformation("{method} executed successfully on {controller}", nameof(GetAll), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(GetAll), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public ActionResult<TDto> Get(TKey id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(Get), GetType().Name, id);
        try
        {
            var res = appService.Get(id);
            logger.LogInformation("{method} executed successfully on {controller}", nameof(Get), GetType().Name);
            return res != null ? Ok(res) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(Get), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }
}
