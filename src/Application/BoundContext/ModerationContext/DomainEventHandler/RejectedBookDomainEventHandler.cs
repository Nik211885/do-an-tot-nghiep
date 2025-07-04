using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class RejectedBookDomainEventHandler(
    ILogger<ApprovedBookDomainEventHandler> logger,
    IEventBus<RejectBookIntegrationEvent> eventBus)
    : IEventHandler<RejectedBookDomainEvent>
{
    private readonly ILogger<ApprovedBookDomainEventHandler> _logger = logger;
    private readonly IEventBus<RejectBookIntegrationEvent> _eventBus = eventBus;
    public async Task Handler(RejectedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start reject book domain event handler make convert to integration event for approval {@Id}", domainEvent.Approval.Id);
        var integrationEvent =
            new RejectBookIntegrationEvent(domainEvent.Approval.ChapterId, domainEvent.Approval.AuthorId, domainEvent.Approval.Decision.Last().Note ?? string.Empty, domainEvent.Approval.BookId);
        _logger.LogInformation("Star reject boook publish to event bus");
        await _eventBus.Publish(integrationEvent, cancellationToken);
    }
}
