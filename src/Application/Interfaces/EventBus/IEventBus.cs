using Application.Models.EventBus;

namespace Application.Interfaces.EventBus;

public interface IEventBus<in TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    Task Publish(TIntegrationEvent @event, CancellationToken cancellationToken = default);
}
