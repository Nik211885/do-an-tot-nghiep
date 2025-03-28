using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services.Repository;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;
    private readonly ApplicationDbContext _dbContext = dbContext;
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        => _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        => await _dbContext.SaveChangesAsync(cancellationToken);

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

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await this._dbContext.DisposeAsync();
    }
}
