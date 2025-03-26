using Core.Events;

namespace Application.Interfaces.CQRS;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IDomainEventHandler<in TDomainEvent> 
    where TDomainEvent : AbsDomainEvent
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Publish(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
