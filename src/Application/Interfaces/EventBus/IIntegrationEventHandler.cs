using Application.Models.EventBus;

namespace Application.Interfaces.EventBus;

public interface IIntegrationEventHandler<in TIntegrationEvent> where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}
