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
}
