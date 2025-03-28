using Core.Events;

namespace Application.Interfaces.CQRS;

/// <summary>
///     Handler for domain event
/// </summary>
/// <typeparam name="TDomainEvent"></typeparam>
public interface IEventHandler<in TDomainEvent> 
    where TDomainEvent : IEvent
{
    /// <summary>
    ///     Defined handler for domain event
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Handler(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
