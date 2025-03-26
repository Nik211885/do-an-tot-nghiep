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
    private List<AbsDomainEvent>? _eventDomains;
    /// <summary>
    ///     
    /// </summary>
    /// <param name="eventDomain"></param>
    public void RaiseDomainEvent(AbsDomainEvent @eventDomain)
    {
        _eventDomains ??= [];
        _eventDomains.Add(@eventDomain);
    }
    /// <summary>
    ///     
    /// </summary>
    public IReadOnlyCollection<AbsDomainEvent>? DomainEvents => _eventDomains;
    /// <summary>
    ///     
    /// </summary>
    public void ClearDomainEvents() => _eventDomains?.Clear();
}
