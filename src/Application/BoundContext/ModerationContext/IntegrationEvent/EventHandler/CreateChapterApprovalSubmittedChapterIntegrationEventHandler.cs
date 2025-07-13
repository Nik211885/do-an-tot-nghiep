using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class CreateChapterApprovalSubmittedChapterIntegrationEventHandler(
    IFactoryHandler factoryHandler,
    ILogger<CreateChapterApprovalSubmittedChapterIntegrationEventHandler> logger)
    : IIntegrationEventHandler<SubmittedAndReviewedChapterVersionIntegrationEvent>
{
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    private readonly ILogger<CreateChapterApprovalSubmittedChapterIntegrationEventHandler> _logger = logger;
    public async Task Handle(SubmittedAndReviewedChapterVersionIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Create chapter has submmited and review has id {@Id}", @event.ChapterId);
        CreateChapterApprovalCommand createChapterApprovalCommand
            = new CreateChapterApprovalCommand(@event.BookId, @event.ChapterId, @event.Content, @event.ChapterTitle, @event.ChapterNumber, @event.ChapterSlug);
        _ = await _factoryHandler.Handler<CreateChapterApprovalCommand, ChapterApprovalViewModel>(createChapterApprovalCommand, cancellationToken);
    }
}
