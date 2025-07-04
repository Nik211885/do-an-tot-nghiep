using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class DeletedBookToIntegrationDomainEventHandler(
    ILogger<DeletedBookToIntegrationDomainEventHandler> logger,
    IEventBus<DeletedBookIntegrationEvent> eventBus)
    : IEventHandler<DeletedBookDomainEvent>
{
    private readonly ILogger<DeletedBookToIntegrationDomainEventHandler> _logger = logger;
    private readonly IEventBus<DeletedBookIntegrationEvent> _eventBus = eventBus;
    public async Task Handler(DeletedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integration = new DeletedBookIntegrationEvent(domainEvent.Book.Id);
        await _eventBus.Publish(integration, cancellationToken);
    }
}
