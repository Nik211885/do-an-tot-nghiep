using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class DeleteChapterApprovalIntegrationEventHandler(
    ILogger<DeleteChapterApprovalIntegrationEventHandler> logger,
    IChapterApprovalRepository chapterApprovalRepository)
    : IIntegrationEventHandler<DeletedChapterIntegrationEvent>
{
    private readonly ILogger<DeleteChapterApprovalIntegrationEventHandler> _logger = logger;
    private readonly IChapterApprovalRepository _chapterApprovalRepository = chapterApprovalRepository;

    public async Task Handle(DeletedChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var chapterApproval = await _chapterApprovalRepository.FindByChapterIdAsync(@event.ChapterId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(chapterApproval, "Chuong");
        _chapterApprovalRepository.Remove(chapterApproval);
        await _chapterApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
