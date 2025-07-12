using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.EventHandler;

public class RejectedBookChangeStatusToRejectedForChapterIntegrationEventHandler(
    ILogger<RejectedBookChangeStatusToRejectedForChapterIntegrationEventHandler> logger,
    IChapterRepository chapterRepository)
    : IIntegrationEventHandler<RejectedChapterIntegrationEvent>
{

    private readonly ILogger<RejectedBookChangeStatusToRejectedForChapterIntegrationEventHandler> _logger = logger;
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    public async Task Handle(RejectedChapterIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var chapter = await _chapterRepository.FindByIdAsync(@event.ChapterId, cancellationToken);
        if (chapter is null)
        {
            return;
        }
        chapter.ModerationRejected();
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
