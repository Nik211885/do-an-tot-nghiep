using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class ChangeTimeForBookApprovalDomainEventHandler(
    ILogger<ChangeTimeForBookApprovalDomainEventHandler> logger,
    IBookApprovalRepository bookApprovalRepository)
    : IEventHandler<CreatedChapterApprovalDomainEvent>,
        IEventHandler<OpenedApprovalDomainEvent>
{
    private readonly ILogger<ChangeTimeForBookApprovalDomainEventHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task Handler(CreatedChapterApprovalDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Handler(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
    }

    public async Task Handler(OpenedApprovalDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await Handler(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
    }
    private async Task Handler(Guid bookApprovalId, CancellationToken cancellationToken)
    {
        BookApproval? bookApproval = await _bookApprovalRepository
            .FindByIdAsync(bookApprovalId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Sach");
        bookApproval.ChangeTimeToApproval();
        _logger.LogInformation("Change time for book approval for time {@Timmer} and book id {@Id}", DateTimeOffset.UtcNow, bookApprovalId);
        _bookApprovalRepository.Update(bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
