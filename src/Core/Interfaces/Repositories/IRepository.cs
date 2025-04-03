namespace Core.Interfaces.Repositories;
/// <summary>
///     Repository it bound action migrations to db is unitOfWork
///     it defined in bound context with aggregate root interface
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    ///  Action make all migrations to database
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
    // add something action like add, delete, and update
    /// <summary>
    ///  Delete state entity is insert
    /// </summary>
    /// <param name="entity"></param>
    T AddEntity(T entity);
    /// <summary>
    /// Update state entity is updated
    /// </summary>
    /// <param name="entity"></param>
    T UpdateEntity(T entity);
    /// <summary>
    /// Update state entity is deleted
    /// </summary>
    /// <param name="entity"></param>
    void DeleteEntity(T entity);
}
