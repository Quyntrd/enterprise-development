namespace BicycleRental.Domain;

/// <summary>
/// Generic repository interface used by application services to perform data access.
/// </summary>
/// <typeparam name="TEntity">Domain entity type.</typeparam>
/// <typeparam name="TKey">Type of the entity key (for this project typically <c>int</c>).</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Create a new entity in the repository.
    /// </summary>
    /// <param name="entity">Entity to create.</param>
    /// <returns>The created entity (may include assigned key).</returns>
    TEntity Create(TEntity entity);

    /// <summary>
    /// Update an existing entity in the repository.
    /// </summary>
    /// <param name="entity">Entity with updated values (must contain identifier).</param>
    void Update(TEntity entity);

    /// <summary>
    /// Delete an entity by identifier.
    /// </summary>
    /// <param name="id">Identifier of the entity to delete.</param>
    void Delete(TKey id);

    /// <summary>
    /// Read an entity by identifier.
    /// </summary>
    /// <param name="id">Identifier of the entity to read.</param>
    /// <returns>The entity if found; otherwise <c>null</c>.</returns>
    TEntity? Read(TKey id);

    /// <summary>
    /// Read all entities from the repository.
    /// </summary>
    /// <returns>List of all entities.</returns>
    List<TEntity> ReadAll();
}
