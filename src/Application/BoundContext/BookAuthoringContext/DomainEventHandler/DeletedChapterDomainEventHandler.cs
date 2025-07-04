using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class DeletedChapterDomainEventHandler(
    IEventBus<DeletedChapterIntegrationEvent> eventBus,
    ILogger<DeletedChapterDomainEventHandler> logger)
    : IEventHandler<DeletedChapterDomainEvent>
{
    private readonly IEventBus<DeletedChapterIntegrationEvent> _eventBus = eventBus;
    private readonly ILogger<DeletedChapterDomainEventHandler> _logger = logger;

    public async Task Handler(DeletedChapterDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var deletedChapterIntegration =
            new DeletedChapterIntegrationEvent(domainEvent.Chapter.Id, domainEvent.Chapter.BookId,
                domainEvent.Chapter.Status);
        await _eventBus.Publish(deletedChapterIntegration, cancellationToken);
    }
}
