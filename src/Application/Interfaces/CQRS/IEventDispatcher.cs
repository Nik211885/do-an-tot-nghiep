using Core.Events;

namespace Application.Interfaces.CQRS;
/// <summary>
///     Dispatch event in application
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    ///     Dispatch event in application
    /// </summary>
    /// <param name="events">event want to dispatch</param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    ///     Dispatch all event 
    /// </returns>
    Task Dispatch(IReadOnlyCollection<IEvent> events, CancellationToken cancellationToken);
}
