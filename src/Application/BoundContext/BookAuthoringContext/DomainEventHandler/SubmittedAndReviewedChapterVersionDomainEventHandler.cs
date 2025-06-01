using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class SubmittedAndReviewedChapterVersionDomainEventHandler
    : IEventHandler<SubmittedAndReviewedChapterVersionDomainEvent>
{
    private readonly ILogger<SubmittedAndReviewedChapterVersionDomainEvent> _logger;
    private readonly IEventBus<SubmittedAndReviewedChapterVersionIntegrationEvent> _eventBus;

    public SubmittedAndReviewedChapterVersionDomainEventHandler(IEventBus<SubmittedAndReviewedChapterVersionIntegrationEvent> eventBus,ILogger<SubmittedAndReviewedChapterVersionDomainEvent> logger)
    {
        _logger = logger;
        _eventBus = eventBus;
    }
    public async Task Handler(SubmittedAndReviewedChapterVersionDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvents = new SubmittedAndReviewedChapterVersionIntegrationEvent(
            chapterId: domainEvent.ChapterVersionId,
            bookId: domainEvent.BookId,
            authorId: domainEvent.AuthorId,
            content: domainEvent.Content
        );
        await _eventBus.Publish(integrationEvents, cancellationToken);
    }
}
