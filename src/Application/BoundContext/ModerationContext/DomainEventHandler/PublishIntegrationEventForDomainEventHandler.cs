using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class PublishIntegrationEventForDomainEventHandler(
    IEventBus<ActivatedBookIntegrationEvent> activatedBookIntegrationEventBus,
    IEventBus<ApprovedChapterIntegrationEvent> approvedChapterIntegrationEventBus,
    IEventBus<CreatedChapterApprovalIntegrationEvent> createdChapterApprovalIntegrationEventBus,
    IEventBus<OpenedApprovalChapterIntegrationEvent> openedChapterIntegrationEventBus,
    IEventBus<RejectedChapterIntegrationEvent> rejectedChapterIntegrationEventBus,
    IEventBus<UnactivatedBookIntegrationEvent> unactivatedBookIntegrationEventBus,
    IBookApprovalRepository bookApprovalRepository)
    : IEventHandler<ActivatedBookDomainEvent>,
        IEventHandler<ApprovedChapterDomainEvent>,
        IEventHandler<CreatedChapterApprovalDomainEvent>,
        IEventHandler<OpenedApprovalDomainEvent>,
        IEventHandler<RejectedChapterDomainEvent>,
        IEventHandler<UnActivatedBookDomainEvent>
{
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    private readonly IEventBus<ActivatedBookIntegrationEvent> _activatedBookIntegrationEventBus = activatedBookIntegrationEventBus;
    private readonly IEventBus<ApprovedChapterIntegrationEvent> _approvedChapterIntegrationEventBus = approvedChapterIntegrationEventBus;
    private readonly IEventBus<CreatedChapterApprovalIntegrationEvent> _createdChapterApprovalIntegrationEventBus = createdChapterApprovalIntegrationEventBus;
    private readonly IEventBus<OpenedApprovalChapterIntegrationEvent>  _openedChapterIntegrationEventBus = openedChapterIntegrationEventBus;
    private readonly IEventBus<RejectedChapterIntegrationEvent> _rejectedChapterIntegrationEventBus = rejectedChapterIntegrationEventBus;
    private readonly IEventBus<UnactivatedBookIntegrationEvent> _unactivatedBookIntegrationEventBus = unactivatedBookIntegrationEventBus;
    public async Task Handler(ActivatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _activatedBookIntegrationEventBus.Publish(new ActivatedBookIntegrationEvent(domainEvent.BookApproval.BookId, domainEvent.BookApproval.AuthorId), cancellationToken);
    }

    public async Task Handler(ApprovedChapterDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookApproval =
            await _bookApprovalRepository.FindByIdAsync(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
        if (bookApproval is not null)
        {
            await _approvedChapterIntegrationEventBus.Publish(new ApprovedChapterIntegrationEvent(bookApproval.BookId,
                domainEvent.ChapterApproval.ChapterId, domainEvent.ChapterApproval.Decision.First().Note ?? string.Empty,
                bookApproval.AuthorId), cancellationToken);
        }
    }

    public async Task Handler(CreatedChapterApprovalDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookApproval =
            await _bookApprovalRepository.FindByIdAsync(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
        if (bookApproval is not null)
        {
            await _createdChapterApprovalIntegrationEventBus.Publish(new CreatedChapterApprovalIntegrationEvent(bookApproval.BookId,
                domainEvent.ChapterApproval.ChapterId, 
                bookApproval.AuthorId), cancellationToken);
        }
    }

    public async Task Handler(OpenedApprovalDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookApproval =
            await _bookApprovalRepository.FindByIdAsync(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
        if (bookApproval is not null)
        {
            await _openedChapterIntegrationEventBus.Publish(new OpenedApprovalChapterIntegrationEvent(bookApproval.BookId,
                domainEvent.ChapterApproval.ChapterId, 
                bookApproval.AuthorId), cancellationToken);
        }
    }

    public async Task Handler(RejectedChapterDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookApproval =
            await _bookApprovalRepository.FindByIdAsync(domainEvent.ChapterApproval.BookApprovalId, cancellationToken);
        if (bookApproval is not null)
        {
            await _rejectedChapterIntegrationEventBus.Publish(new RejectedChapterIntegrationEvent(bookApproval.BookId,
                domainEvent.ChapterApproval.ChapterId,  domainEvent.ChapterApproval.Decision.First().Note ?? string.Empty,
                bookApproval.AuthorId), cancellationToken);
        }
    }

    public async Task Handler(UnActivatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await _unactivatedBookIntegrationEventBus.Publish(new UnactivatedBookIntegrationEvent(domainEvent.BookApproval.BookId, domainEvent.BookApproval.AuthorId), cancellationToken);
    }
}
