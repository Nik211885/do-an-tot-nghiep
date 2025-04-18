﻿using System.Reflection;
using Core.Entities.TestAggregate;
using Core.Interfaces;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data;
/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
/// <param name="dbConnectionStringSelector"></param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
    IDbConnectionStringSelector dbConnectionStringSelector)
    : DbContext(options), IUnitOfWork
{
    /// <summary>
    /// 
    /// </summary>
    private IDbContextTransaction? _transaction;
    /// <summary>
    /// 
    /// </summary>
    private readonly IDbConnectionStringSelector _dbConnectionStringSelector = dbConnectionStringSelector;
    /// <summary>
    /// 
    /// </summary>
    private bool _isReadOnly = false;
    /// <summary>
    /// 
    /// </summary>
    public DbSet<TestCaseAggregate> TestCases { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ApplicationDbContext ReadOnly()
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
        var connectionString = _isReadOnly
            ? _dbConnectionStringSelector.GetSlaveDbConnectionString()
            : _dbConnectionStringSelector.GetMasterDbConnectionString();
        ArgumentNullException.ThrowIfNull(connectionString);
        optionsBuilder.UseNpgsql(connectionString);
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
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
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
        await _transaction.DisposeAsync();
    }
    /// <summary>
    /// 
    /// </summary>
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
    }
}
