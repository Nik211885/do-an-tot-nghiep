using Core.Events;

namespace Application.Interfaces.CQRS;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IEventHandler<in TDomainEvent> 
    where TDomainEvent : IEvent
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Handler(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
