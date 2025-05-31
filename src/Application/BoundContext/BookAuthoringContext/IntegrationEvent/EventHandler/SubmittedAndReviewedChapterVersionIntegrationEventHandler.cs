using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.EventHandler;

public class SubmittedAndReviewedChapterVersionIntegrationEventHandler
    : IIntegrationEventHandler<SubmittedAndReviewedChapterVersionIntegrationEvent>
{
    private readonly ILogger<SubmittedAndReviewedChapterVersionIntegrationEventHandler> _logger;

    public SubmittedAndReviewedChapterVersionIntegrationEventHandler(ILogger<SubmittedAndReviewedChapterVersionIntegrationEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(SubmittedAndReviewedChapterVersionIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Star consumer submit and revied chapter version id {@chapterVersison}", @event);
        return Task.CompletedTask;  
    }
}
