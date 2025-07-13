using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Interfaces.EventBus;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class DeleteBookApprovalIntegrationEventHandler
    : IIntegrationEventHandler<DeletedBookIntegrationEvent>
{
    public Task Handle(DeletedBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
