using Application.Interfaces.CQRS;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class AutoModerationOnChapterReadyDomainEventHandler
    : IEventHandler<ChapterReadyForModerationDomainEvent>
{
    private readonly ILogger<AutoModerationOnChapterReadyDomainEventHandler> _logger;

    public AutoModerationOnChapterReadyDomainEventHandler(ILogger<AutoModerationOnChapterReadyDomainEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handler(ChapterReadyForModerationDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start moderation auto {@moderation}", domainEvent.BookApproval);
        return Task.CompletedTask;
    }
}
