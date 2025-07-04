using Application.Interfaces.CQRS;
using Core.Entities;
using Core.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;
/// <summary>
/// 
/// </summary>
/// <param name="eventDispatcher"></param>
public class DispatcherDomainEventInterceptors(IEventDispatcher eventDispatcher) : SaveChangesInterceptor
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IEventDispatcher _eventDispatcher =  eventDispatcher;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        // turn on all domain event after save change state in entity
        await DispatchDomainEvents(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    private async Task DispatchDomainEvents(Microsoft.EntityFrameworkCore.DbContext? context, CancellationToken cancellationToken)
    {
        if (context == null) return;
        
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents is not null && x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();
        var events = entities
            .SelectMany(e => e.DomainEvents!)
            .ToList();
        try
        {
            await _eventDispatcher.Dispatch(events.AsReadOnly(), cancellationToken);
            
            foreach (var entity in entities)
            {
                entity.ClearDomainEvents();
            }
        }
        catch (Exception)
        {
            throw; 
        }
    }
}
