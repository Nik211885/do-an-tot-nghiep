using Application.Interfaces.CQRS;
using Core.Events;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Services.CQRS;
/// <summary>
/// /
/// </summary>
/// <param name="serviceProvider"></param>

public class EventDispatcher(IServiceProvider serviceProvider) 
    : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="events"></param>
    /// <param name="cancellationToken"></param>
    public async Task Dispatch(IReadOnlyCollection<IEvent> events, 
        CancellationToken cancellationToken)
    {
        await Parallel.ForEachAsync(events, cancellationToken, async (@event, ct) =>
        {
            var eventType = @event.GetType();
            var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            var eventHandlers = _serviceProvider.GetServices(eventHandlerType);
            foreach (var eventHandler in eventHandlers)
            {
                var handleMethod = eventHandlerType.GetMethod("Handler");
                if (handleMethod == null)
                {
                    continue;
                }

                if (handleMethod.Invoke(eventHandler, [@event, ct]) is not Task task)
                {
                    return;
                }
                await task;
            }
        });
    }
}
