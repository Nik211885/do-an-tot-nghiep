using Application.Interfaces.CQRS;
using Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.CQRS;
public class EventDispatcher(
    IServiceProvider serviceProvider, 
    ILogger<EventDispatcher> logger)
    : IEventDispatcher
{
    public async Task Dispatch(IReadOnlyCollection<IEvent> events, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();

        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            var handlerInstances = serviceProvider.GetServices(handlerType);
            foreach (var handler in handlerInstances)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        using var scope = serviceProvider.CreateScope();
                        
                        var scopedHandler = scope.ServiceProvider.GetRequiredService(handlerType);
                        var handleMethod = handlerType.GetMethod("Handler");

                        if (handleMethod != null)
                        {
                            var result = handleMethod.Invoke(scopedHandler, [@event, cancellationToken]);

                            if (result is Task taskResult)
                                await taskResult;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error when handling domain event {EventType}", eventType.Name);
                    }
                }, cancellationToken));
            }
        }

        await Task.WhenAll(tasks);
    }
}
