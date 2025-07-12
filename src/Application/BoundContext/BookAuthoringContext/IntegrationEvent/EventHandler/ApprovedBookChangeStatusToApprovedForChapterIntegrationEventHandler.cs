using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.EventHandler;

public class ApprovedBookChangeStatusToApprovedForChapterIntegrationEventHandler(
    ILogger<ApprovedBookChangeStatusToApprovedForChapterIntegrationEventHandler> logger,
    IChapterRepository chapterRepository)
    : IIntegrationEventHandler<ApprovedChapterIntegrationEvent>
{
    private readonly ILogger<ApprovedBookChangeStatusToApprovedForChapterIntegrationEventHandler> _logger = logger;
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    public async Task Handle(ApprovedChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var chapter = await _chapterRepository.FindByIdAsync(@event.ChapterId, cancellationToken);
        if (chapter is null)
        {
            return;
        }
        chapter.ModerationApproved();
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
