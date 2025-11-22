namespace BicycleRental.Domain;

/// <summary>
/// Generic asynchronous repository interface used by application services to perform data access.
/// </summary>
/// <typeparam name="TEntity">Domain entity type</typeparam>
/// <typeparam name="TKey">Key type (int)</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Create a new entity and persist it.
    /// </summary>
    /// <param name="entity">Entity to create.</param>
    /// <returns>Created entity (with generated key).</returns>
    public Task<TEntity> Create(TEntity entity);

    /// <summary>
    /// Update existing entity.
    /// </summary>
    /// <param name="entity">Entity with updated values.</param>
    public Task<TEntity> Update(TEntity entity);

    /// <summary>
    /// Delete entity by id.
    /// </summary>
    /// <param name="id">Identifier of entity.</param>
    public Task<bool> Delete(TKey id);

    /// <summary>
    /// Read entity by id.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <returns>Entity or null if not found.</returns>
    public Task<TEntity?> Read(TKey id);

    /// <summary>
    /// Read all entities.
    /// </summary>
    /// <returns>List of entities.</returns>
    public Task<IList<TEntity>> ReadAll();
}
