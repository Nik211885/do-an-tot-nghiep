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
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispatchDomainEvents(eventData.Context, CancellationToken.None).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    private async Task DispatchDomainEvents(DbContext? context, CancellationToken cancellationToken)
    {
        if (context == null) return;
        
        var eventBases = context.ChangeTracker.Entries<AbsBaseEntity>()
            .Where(x => x.Entity.DomainEvents is not null 
                        && x.Entity.DomainEvents.Any())
            .SelectMany(x => x.Entity.DomainEvents 
                             ?? Enumerable.Empty<IEvent>()).ToList().AsReadOnly();
        if (eventBases.Any())
        {
            await _eventDispatcher.Dispatch(eventBases,cancellationToken);
        }
    }
}
