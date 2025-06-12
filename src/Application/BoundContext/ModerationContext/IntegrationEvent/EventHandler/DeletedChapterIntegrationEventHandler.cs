using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class DeletedChapterIntegrationEventHandler(
    IBookApprovalRepository bookApprovalRepository,
    ILogger<DeletedChapterIntegrationEventHandler> logger)
    : IIntegrationEventHandler<DeletedChapterIntegrationEvent>
{
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    private readonly ILogger<DeletedChapterIntegrationEventHandler> _logger = logger;

    public async Task Handle(DeletedChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var bookApproval = await _bookApprovalRepository.
            FindByBookIdAndChapterIdAsync(@event.BookId, @event.ChapterId, cancellationToken);
        if (bookApproval is null)
        {
            _logger.LogInformation("Chapter has send submmit and review for {@BookId}", @event.BookId);
            return;
        }
        bookApproval.DeletedApproved();
        _bookApprovalRepository.Delete(bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
