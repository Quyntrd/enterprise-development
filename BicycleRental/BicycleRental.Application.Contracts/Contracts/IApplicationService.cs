namespace BicycleRental.Application.Contracts.Contracts;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Generic application service interface for CRUD operations.
/// </summary>
/// <typeparam name="TDto">DTO used for GET responses</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO used for POST/PUT requests</typeparam>
/// <typeparam name="TKey">Type of identifier (int, Guid, etc.)</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Create entity from DTO
    /// </summary>
    /// <param name="dto">Create DTO</param>
    /// <returns>Created DTO</returns>
    public Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Get entity by id
    /// </summary>
    /// <param name="dtoId">Identifier</param>
    /// <returns>Returned DTO</returns>
    public Task<TDto> Get(TKey dtoId);

    /// <summary>
    /// Get all entities
    /// </summary>
    /// <returns>List of DTOs</returns>
    public Task<List<TDto>> GetAll();

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="dto">Update DTO</param>
    /// <param name="dtoId">Identifier to update</param>
    /// <returns>Updated DTO</returns>
    public Task<TDto> Update(TCreateUpdateDto dto, TKey dtoId);

    /// <summary>
    /// Delete entity by id
    /// </summary>
    /// <param name="dtoId">Identifier</param>
    public Task<bool> Delete(TKey dtoId);
}
