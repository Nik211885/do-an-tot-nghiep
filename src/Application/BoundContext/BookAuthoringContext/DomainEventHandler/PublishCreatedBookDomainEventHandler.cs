using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class PublishCreatedBookDomainEventHandler(
    ILogger<PublishCreatedBookDomainEventHandler> logger,
    IEventBus<CreatedBookIntegrationEvent> eventBus)
    : IEventHandler<CreatedBookDomainEvent>
{
    private readonly ILogger<PublishCreatedBookDomainEventHandler> _logger = logger;
    private readonly IEventBus<CreatedBookIntegrationEvent> _eventBus = eventBus;
    public async Task Handler(CreatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started publishing new book domain event");
        var integrationEvent = new CreatedBookIntegrationEvent(domainEvent.Book);
        await _eventBus.Publish(integrationEvent, cancellationToken);
    }
}
