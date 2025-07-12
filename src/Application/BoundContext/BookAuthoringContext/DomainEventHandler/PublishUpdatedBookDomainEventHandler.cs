using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class PublishUpdatedBookDomainEventHandler(
    ILogger<PublishUpdatedBookDomainEventHandler> logger,
    IEventBus<UpdatedBookIntegrationEvent> eventBus)
    : IEventHandler<UpdatedBookDomainEvent>
{
    public async Task Handler(UpdatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Started publishing new book domain event");
        var updateBook = new UpdatedBookIntegrationEvent(domainEvent.Book);
        await eventBus.Publish(updateBook, cancellationToken);
    }
}
