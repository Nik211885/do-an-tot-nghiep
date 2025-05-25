using Application.Interfaces.CQRS;
using Core.Events;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.CQRS
{
    /// <summary>
    /// Dispatcher event
    /// </summary>
    public class EventDispatcher(IServiceProvider serviceProvider,
        ILogger<EventDispatcher> logger) 
        : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<EventDispatcher> _logger = logger;

        public async Task Dispatch(IReadOnlyCollection<IEvent> events, CancellationToken cancellationToken)
        {
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10, CancellationToken = cancellationToken };

            await Parallel.ForEachAsync(events, parallelOptions, async (@event, ct) =>
            {
                var eventType = @event.GetType();
                var eventHandlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

                var eventHandlers = _serviceProvider.GetServices(eventHandlerType);

                var handlerTasks = eventHandlers.Select(eventHandler =>
                {
                    var handleMethod = eventHandlerType.GetMethod("Handler");
                    if (handleMethod == null)
                        return Task.CompletedTask;

                    try
                    {
                        var result = handleMethod.Invoke(eventHandler, [@event, ct]);
                        return result as Task ?? Task.CompletedTask;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,"Can't not invoke event handler in {@Handler}", handleMethod);
                        return Task.CompletedTask;
                    }
                });

                await Task.WhenAll(handlerTasks);
            });
        }
    }
}
