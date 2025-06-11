using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class ApprovedBookDomainEventHandler(
    ILogger<ApprovedBookDomainEventHandler> logger,
    IEventBus<ApprovalBookIntegrationEvent> eventBus)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly ILogger<ApprovedBookDomainEventHandler> _logger = logger;
    private readonly IEventBus<ApprovalBookIntegrationEvent> _eventBus = eventBus;
    public async Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start approval book domain event handler make convert to integration event for approval {@Id}", domainEvent.Approval.Id);
        var integrationEvent =
            new ApprovalBookIntegrationEvent(domainEvent.Approval.ChapterId, domainEvent.Approval.AuthorId, domainEvent.Approval.BookId);
        _logger.LogInformation("Star approval book publish to event bus");
        await _eventBus.Publish(integrationEvent, cancellationToken);
    }
}
