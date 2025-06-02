using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Application.Interfaces.ProcessData;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class SubmittedAndReviewedChapterVersionDomainEventHandler(
    IEventBus<SubmittedAndReviewedChapterVersionIntegrationEvent> eventBus,
    ILogger<SubmittedAndReviewedChapterVersionDomainEvent> logger,
    ICleanTextService cleanTextService)
    : IEventHandler<SubmittedAndReviewedChapterVersionDomainEvent>
{
    private readonly ILogger<SubmittedAndReviewedChapterVersionDomainEvent> _logger = logger;
    private readonly ICleanTextService _cleanTextService = cleanTextService;
    private readonly IEventBus<SubmittedAndReviewedChapterVersionIntegrationEvent> _eventBus = eventBus;
    public async Task Handler(SubmittedAndReviewedChapterVersionDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvents = new SubmittedAndReviewedChapterVersionIntegrationEvent(
            chapterId: domainEvent.ChapterVersionId,
            bookId: domainEvent.BookId,
            authorId: domainEvent.AuthorId,
            content: _cleanTextService.RemoveHtmlTag(domainEvent.Content)
        );
        await _eventBus.Publish(integrationEvents, cancellationToken);
    }
}
