using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Application.Interfaces.ProcessData;
using Core.Events.BookAuthoringContext;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class SubmittedAndReviewedChapterVersionDomainEventHandler(
    IEventBus<SubmittedAndReviewedChapterVersionIntegrationEvent> eventBus,
    ILogger<SubmittedAndReviewedChapterVersionDomainEvent> logger,
    IBookRepository bookRepository,
    ICleanTextService cleanTextService)
    : IEventHandler<SubmittedAndReviewedChapterVersionDomainEvent>
{
    private readonly ILogger<SubmittedAndReviewedChapterVersionDomainEvent> _logger = logger;
    private readonly ICleanTextService _cleanTextService = cleanTextService;
    private readonly IBookRepository _bookRepository= bookRepository;
    private readonly IEventBus<SubmittedAndReviewedChapterVersionIntegrationEvent> _eventBus = eventBus;
    public async Task Handler(SubmittedAndReviewedChapterVersionDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(domainEvent.BookId, cancellationToken);
        if (book is null)
        {
            return;
        }
        var integrationEvents = new SubmittedAndReviewedChapterVersionIntegrationEvent(
            chapterId: domainEvent.ChapterId,
            bookId: domainEvent.BookId,
            authorId: book.CreatedUerId,
            content: domainEvent.Content,
            chapterTitle: domainEvent.ChapterTitle,
            bookTitle: book.Title
        );
        await _eventBus.Publish(integrationEvents, cancellationToken);
    }
}
