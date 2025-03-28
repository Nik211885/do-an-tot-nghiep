using System.Reflection;
using Application.Interfaces.CQRS;
using Core.Entities;
using Core.Entities.TestAggregate;
using Core.Events;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
/// <summary>
/// 
/// </summary>
/// <param name="eventDispatcher"></param>
/// <param name="options"></param>
/// <param name="dbConnectionStringSelector"></param>
public class ApplicationDbContext(IEventDispatcher eventDispatcher, 
    DbContextOptions<ApplicationDbContext> options, IDbConnectionStringSelector dbConnectionStringSelector) 
    : DbContext(options)
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;
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
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
         var resultSaveChanges = await base.SaveChangesAsync(cancellationToken);
         var eventBases = ChangeTracker.Entries<AbsBaseEntity>()
             .Where(x => x.Entity.DomainEvents is not null 
                         && x.Entity.DomainEvents.Any())
             .SelectMany(x => x.Entity.DomainEvents 
                              ?? Enumerable.Empty<IEvent>()).ToList().AsReadOnly();
         if (eventBases.Any())
         {
             await _eventDispatcher.Dispatch(eventBases,cancellationToken);
         }
         return resultSaveChanges;
    }
}
