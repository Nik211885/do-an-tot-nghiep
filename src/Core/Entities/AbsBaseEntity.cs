using Core.Events;
using Core.Interfaces;

namespace Core.Entities;
/// <summary>
///     Base entity in core just have id and event domain
/// </summary>
public abstract class AbsBaseEntity
{
    /// <summary>
    ///     Identity for entity it follows id is uuid7
    /// </summary>
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    /// <summary>
    ///     
    /// </summary>
    private List<IEvent>? _eventDomains;
    /// <summary>
    ///     
    /// </summary>
    /// <param name="event"></param>
    public void RaiseDomainEvent(IEvent @event)
    {
        _eventDomains ??= [];
        _eventDomains.Add(@event);
    }
    /// <summary>
    ///     
    /// </summary>
    public IReadOnlyCollection<IEvent>? DomainEvents => _eventDomains;
    /// <summary>
    ///     
    /// </summary>
    public void ClearDomainEvents() => _eventDomains?.Clear();
}
