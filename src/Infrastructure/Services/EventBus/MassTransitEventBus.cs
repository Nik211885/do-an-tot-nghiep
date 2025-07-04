using Application.Interfaces.EventBus;
using Application.Models.EventBus;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.EventBus;

public class MassTransitEventBus<TIntegrationEvent>(IPublishEndpoint publishEndpoint,
    ILogger<MassTransitEventBus<TIntegrationEvent>> logger) 
    : IEventBus<TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    private readonly IPublishEndpoint _publishEndpoint =publishEndpoint;
    private readonly ILogger<MassTransitEventBus<TIntegrationEvent>> _logger = logger;

    public async Task Publish(TIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }
}
