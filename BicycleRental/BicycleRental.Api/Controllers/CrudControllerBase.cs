using BicycleRental.Api.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Generic base controller providing CRUD endpoints.
/// </summary>
/// <typeparam name="TDto">DTO used for GET responses</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO used for POST/PUT requests</typeparam>
/// <typeparam name="TKey">Identifier type</typeparam>
[Route("api/[controller]")]
[ApiController]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TKey>(
    IApplicationService<TDto, TCreateUpdateDto, TKey> appService,
    ILogger<CrudControllerBase<TDto, TCreateUpdateDto, TKey>> logger) : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Create a new resource.
    /// </summary>
    /// <param name="newDto">DTO containing data for the new resource.</param>
    /// <returns>Created DTO with assigned identifier.</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
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
                return StatusCode(500, "Created resource is null.");
            }

            var idProp = res.GetType().GetProperty("Id");
            if (idProp != null)
            {
                var idValue = idProp.GetValue(res);
                if (idValue != null)
                {
                    return CreatedAtAction(nameof(Get), new { id = idValue }, res);
                }
            }

            return Created(string.Empty, res);
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(Create), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(Create), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Update an existing resource.
    /// </summary>
    /// <param name="id">Identifier of the resource to update.</param>
    /// <param name="newDto">DTO containing updated values.</param>
    /// <returns>Updated DTO.</returns>
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
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(Edit), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(Edit), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Delete an existing resource.
    /// </summary>
    /// <param name="id">Identifier of the resource to delete.</param>
    /// <returns>HTTP status indicating outcome.</returns>
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
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(Delete), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception in {method} of {controller}", nameof(Delete), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieve all resources.
    /// </summary>
    /// <returns>List of DTOs representing all resources.</returns>
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

    /// <summary>
    /// Retrieve a single resource by identifier.
    /// </summary>
    /// <param name="id">Identifier of the resource to retrieve.</param>
    /// <returns>DTO if found; otherwise NoContent.</returns>
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
