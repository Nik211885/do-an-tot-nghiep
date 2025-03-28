using System.Reflection;
using Application.Interfaces.CQRS;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(IEventDispatcher eventDispatcher) : DbContext
{
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
         var resultSaveChanges = await base.SaveChangesAsync(cancellationToken);
         var eventBases = ChangeTracker.Entries<AbsBaseEntity>()
             .Where(x => x.Entity.DomainEvents is not null && x.Entity.DomainEvents.Any())
             .Select(x => x.Entity.DomainEvents);
         foreach (var events in eventBases)
         {
             if (events is not null && events.Any())
             {
                 await _eventDispatcher.Dispatch(events, cancellationToken);
             }
         }
         return resultSaveChanges;
    }
}
