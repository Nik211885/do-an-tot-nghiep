﻿namespace Core.Interfaces;
/// <summary>
///     Unit of work it make have one migration to db make performance to application 
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    ///     Create transaction 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken);
    /// <summary>
    ///     Move all change data to database 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// </returns>
    Task<int> SaveChangeAsync(CancellationToken cancellationToken);
    /// <summary>
    ///     When call commit transaction this is will move migrations
    ///     to database and if you have error it will roll back all migrations
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    /// <summary>
    ///     Roll back all data if failure change data
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken);
}
