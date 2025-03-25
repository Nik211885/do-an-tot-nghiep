using Core.Interfaces;

namespace Core.Entities;
/// <summary>
///     Base entity in core just have id and event domain
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    ///     Identity for entity it follows id is uuid7
    /// </summary>
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    /// <summary>
    ///     
    /// </summary>
    private List<IEventDomain>? _eventDomains;
    /// <summary>
    ///     
    /// </summary>
    /// <param name="eventDomain"></param>
    public void RaiseDomainEvent(IEventDomain @eventDomain)
    {
        _eventDomains ??= [];
        _eventDomains.Add(@eventDomain);
    }
    /// <summary>
    ///     
    /// </summary>
    public IReadOnlyCollection<IEventDomain>? DomainEvents => _eventDomains;
    /// <summary>
    ///     
    /// </summary>
    public void ClearDomainEvents() => _eventDomains?.Clear();
}
