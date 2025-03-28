using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services.Repository;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    /// <summary>
    /// 
    /// </summary>
    private IDbContextTransaction? _transaction;
    /// <summary>
    /// 
    /// </summary>
    private readonly ApplicationDbContext _dbContext = dbContext;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        => _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        => await _dbContext.SaveChangesAsync(cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        try
        {
            await this.SaveChangeAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _transaction.RollbackAsync(cancellationToken);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        await _transaction.RollbackAsync(cancellationToken);
    }
    /// <summary>
    /// 
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await this._dbContext.DisposeAsync();
    }
}
