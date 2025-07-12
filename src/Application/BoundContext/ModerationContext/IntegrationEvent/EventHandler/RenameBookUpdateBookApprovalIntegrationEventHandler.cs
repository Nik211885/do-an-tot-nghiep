using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Elastic.Clients.Elasticsearch.Snapshot;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class RenameBookUpdateBookApprovalIntegrationEventHandler(
    ILogger<RenameBookUpdateBookApprovalIntegrationEventHandler> logger,
    IBookApprovalRepository bookApprovalRepository)
    : IIntegrationEventHandler<UpdatedBookIntegrationEvent>
{
    private readonly ILogger<RenameBookUpdateBookApprovalIntegrationEventHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;

    public async Task Handle(UpdatedBookIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var bookApproval = await _bookApprovalRepository.FindByBookIdAsync(integrationEvent.Book.Id, cancellationToken);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(bookApproval,"Sach");
        if (bookApproval.BookTitle == integrationEvent.Book.Title)
        {
            _logger.LogInformation("Rename book when updated book approval don't update tittle dont change");
            return;
        }
        bookApproval.RenameBook(integrationEvent.Book.Title);
        _bookApprovalRepository.Update(bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
