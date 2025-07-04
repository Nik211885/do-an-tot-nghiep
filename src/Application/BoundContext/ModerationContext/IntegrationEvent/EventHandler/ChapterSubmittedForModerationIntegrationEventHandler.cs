using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class ChapterSubmittedForModerationIntegrationEventHandler(
    ILogger<ChapterSubmittedForModerationIntegrationEventHandler> logger,
    IFactoryHandler factoryHandler)
    : IIntegrationEventHandler<SubmittedAndReviewedChapterVersionIntegrationEvent>
{
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    private readonly ILogger<ChapterSubmittedForModerationIntegrationEventHandler> _logger = logger;
    public async Task Handle(SubmittedAndReviewedChapterVersionIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start submitted and reviewed chapter in moderation context  {@event}", @event);
        var command = new CreateBookApprovalCommand(
            BookId: @event.BookId,
            ChapterId: @event.ChapterId,
            ChapterContent: @event.Content,
            AuthorId : @event.AuthorId,
            BookTitle: @event.BookTitle,
            ChapterTitle: @event.ChapterTitle,
            ChapterNumber: @event.ChapterNumber,
            ChapterSlug: @event.ChapterSlug
        );
        await _factoryHandler.Handler<CreateBookApprovalCommand, BookApprovalViewModel>(command, cancellationToken);
        _logger.LogInformation("Chapter has read {@command}", command);
    }
}
