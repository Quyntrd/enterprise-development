using BicycleRental.Application.Contracts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BicycleRental.Api.Controllers;

/// <summary>
/// Generic base controller providing asynchronous CRUD endpoints.
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
    /// Create a new resource asynchronously.
    /// </summary>
    /// <param name="newDto">DTO containing data for the new resource.</param>
    /// <returns>Created DTO with assigned identifier.</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<TDto>> CreateAsync(TCreateUpdateDto newDto)
    {
        logger.LogInformation("{method} method of {controller} is called with {@dto} parameter", nameof(CreateAsync), GetType().Name, newDto);
        try
        {
            var res = await appService.Create(newDto);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(CreateAsync), GetType().Name);

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
                    return CreatedAtAction(nameof(GetAsync), new { id = idValue }, res);
                }
            }

            return Created(string.Empty, res);
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(CreateAsync), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(CreateAsync), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Update an existing resource asynchronously.
    /// </summary>
    /// <param name="id">Identifier of the resource to update.</param>
    /// <param name="newDto">DTO containing updated values.</param>
    /// <returns>Updated DTO.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<TDto>> EditAsync(TKey id, TCreateUpdateDto newDto)
    {
        logger.LogInformation("{method} called on {controller} with id={id} and payload {@dto}", nameof(EditAsync), GetType().Name, id, newDto);
        try
        {
            var res = await appService.Update(newDto, id);
            logger.LogInformation("{method} executed successfully on {controller}", nameof(EditAsync), GetType().Name);
            return Ok(res);
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(EditAsync), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(EditAsync), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Delete an existing resource asynchronously.
    /// </summary>
    /// <param name="id">Identifier of the resource to delete.</param>
    /// <returns>HTTP status indicating outcome.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public virtual async Task<IActionResult> DeleteAsync(TKey id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(DeleteAsync), GetType().Name, id);
        try
        {
            var ok = await appService.Delete(id);
            if (!ok)
            {
                logger.LogInformation("{method} did not find resource on {controller} with id={id}", nameof(DeleteAsync), GetType().Name, id);
                return NotFound();
            }

            logger.LogInformation("{method} executed successfully on {controller}", nameof(DeleteAsync), GetType().Name);
            return Ok();
        }
        catch (ArgumentException argEx)
        {
            logger.LogWarning(argEx, "Validation failed in {method} of {controller}", nameof(DeleteAsync), GetType().Name);
            return BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(DeleteAsync), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieve all resources asynchronously.
    /// </summary>
    /// <returns>List of DTOs representing all resources.</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<IList<TDto>>> GetAllAsync()
    {
        logger.LogInformation("{method} called on {controller}", nameof(GetAllAsync), GetType().Name);
        try
        {
            var res = await appService.GetAll();
            logger.LogInformation("{method} executed successfully on {controller}", nameof(GetAllAsync), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(GetAllAsync), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieve a single resource by identifier asynchronously.
    /// </summary>
    /// <param name="id">Identifier of the resource to retrieve.</param>
    /// <returns>DTO if found; otherwise NoContent.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public virtual async Task<ActionResult<TDto>> GetAsync(TKey id)
    {
        logger.LogInformation("{method} called on {controller} with id={id}", nameof(GetAsync), GetType().Name, id);
        try
        {
            var res = await appService.Get(id);
            logger.LogInformation("{method} executed successfully on {controller}", nameof(GetAsync), GetType().Name);
            return res != null ? Ok(res) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(GetAsync), GetType().Name);
            return StatusCode(500, ex.Message);
        }
    }
}
