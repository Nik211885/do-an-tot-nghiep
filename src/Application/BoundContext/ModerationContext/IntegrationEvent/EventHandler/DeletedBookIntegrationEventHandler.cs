using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class DeletedBookIntegrationEventHandler(
    ILogger<DeletedBookIntegrationEventHandler> logger,
    IBookApprovalRepository bookApprovalRepository)
    : IIntegrationEventHandler<DeletedBookIntegrationEvent>
{
    private readonly ILogger<DeletedBookIntegrationEventHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task Handle(DeletedBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var approval = await _bookApprovalRepository.FindByBookIdAsync(@event.BookId, cancellationToken);
        Parallel.ForEach(approval, ap =>
        {
            ap.DeletedApproved();
        });
        await _bookApprovalRepository.BulkDeleteAsync(approval, cancellationToken);
    }
}
