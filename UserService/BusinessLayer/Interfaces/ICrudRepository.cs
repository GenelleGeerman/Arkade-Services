namespace BusinessLayer.Interfaces;

public interface ICrudRepository<T> where T : class
{
    /// <summary>
    /// Create a entity
    /// </summary>
    /// <param name="entity"></param>
    bool Create(T entity);

    /// <summary>
    /// Get a item by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// object
    /// </returns>
    T GetById(object id);

    /// <summary>
    /// Get all the items
    /// </summary>
    /// <returns>
    /// IEnumerable<typeparamref name="T"/>
    /// </returns>
    List<T> GetAll();

    /// <summary>
    /// Update a entity
    /// </summary>
    /// <param name="entity"></param>
    T Update(T entity);

    /// <summary>
    /// Delete a item from a entity
    /// </summary>
    /// <param name="entity"></param>
    bool Delete(T entity);

    /// <summary>
    /// Delete all entity
    /// </summary>
    bool DeleteAll();
}
