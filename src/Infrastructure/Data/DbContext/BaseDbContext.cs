using Core.Interfaces;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data.DbContext;

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
/// <param name="dbConnectionStringSelector"></param>
public class BaseDbContext(
    DbContextOptions options,
    IDbConnectionStringSelector dbConnectionStringSelector,
    DispatcherDomainEventInterceptors interceptors)
    : Microsoft.EntityFrameworkCore.DbContext(options), IUnitOfWork
{
    /// <summary>
    /// 
    /// </summary>
    private IDbContextTransaction? _transaction;

    private readonly DispatcherDomainEventInterceptors _interceptors = interceptors;

    /// <summary>
    /// 
    /// </summary>
    private readonly IDbConnectionStringSelector _dbConnectionStringSelector = dbConnectionStringSelector;

    /// <summary>
    /// 
    /// </summary>
    private bool _isReadOnly = false;

    /*/// <summary>
    ///
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }*/
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public BaseDbContext ReadOnly()
    {
        _isReadOnly = true;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = GetConnectionString();
        ArgumentNullException.ThrowIfNull(connectionString);
        optionsBuilder.UseNpgsql(connectionString)
            .AddInterceptors(_interceptors);
    }

    /// <summary>
    /// 
    /// </summary>  
    /// <param name="cancellationToken"></param>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        => _transaction = await base.Database.BeginTransactionAsync(cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        => await base.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        try
        {
            await base.SaveChangesAsync(cancellationToken);
            await _transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await _transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(_transaction);
        if (HasActiveTransaction)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool HasActiveTransaction => _transaction is not null;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task ExecutionStrategyRetry(Func<Task> func)
    {
        var strategy = base.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await func();
        });
    }

    public Guid? CurrentTransactionId => _transaction?.TransactionId;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string? GetConnectionString()
        => _isReadOnly
            ? _dbConnectionStringSelector.GetSlaveDbConnectionString()
            : _dbConnectionStringSelector.GetMasterDbConnectionString();

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
    }
}
