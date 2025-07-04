using System.Reflection;
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
        using var scope = serviceProvider.CreateScope();

        foreach (var @event in events)
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
            var handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                var handleMethod = handlerType.GetMethod("Handler");
                if (handleMethod == null) continue;

                if (handler != null)
                {
                    await InvokeHandlerAsync(handleMethod, handler, @event, cancellationToken, eventType.Name);
                }
            }
        }
    }
    private async Task InvokeHandlerAsync(MethodInfo handleMethod, object handler, IEvent @event, CancellationToken cancellationToken, string eventTypeName)
    {
        try
        {
            var result = handleMethod.Invoke(handler, [@event, cancellationToken]);
            if (result is Task taskResult)
                await taskResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when handling domain event {EventType}", eventTypeName);
        }
    }
}
