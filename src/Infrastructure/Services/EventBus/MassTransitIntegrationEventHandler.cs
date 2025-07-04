using Application.Interfaces.EventBus;
using Application.Models.EventBus;
using MassTransit;

namespace Infrastructure.Services.EventBus;

public class MassTransitIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> handler)
    : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    private readonly IIntegrationEventHandler<TIntegrationEvent> _handler = handler;
    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        await _handler.Handle(context.Message, context.CancellationToken);
    }
}
