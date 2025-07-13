using Application.Interfaces.CQRS;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Events.ModerationContext;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class IncrementChapterCountForBookDomainEventHandler(
    ILogger<IncrementChapterCountForBookDomainEventHandler> logger,
    IBookApprovalRepository bookApprovalRepository)
    : IEventHandler<CreatedChapterApprovalDomainEvent>
{
    private readonly ILogger<IncrementChapterCountForBookDomainEventHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task Handler(CreatedChapterApprovalDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        BookApproval? bookApproval = await _bookApprovalRepository.FindByIdAsync(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(bookApproval, "Sach");
        bookApproval.IncrementChapterCount();
        _logger.LogInformation("Increment chapter count for book has id {@Id}", bookApproval.BookId);
        _bookApprovalRepository.Update(bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
