using Application.BoundContext.BookReviewContext.Command.Reader;
using Application.BoundContext.BookReviewContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;

namespace Application.BoundContext.BookReviewContext.IntegrationEvent.EventHandler;

public class ReadBookIntegrationEventHandler(IFactoryHandler factoryHandler)
    : IIntegrationEventHandler<ReadeBookIntegrationEvent>
{
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    public async Task Handle(ReadeBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var command = new CreateReaderCommand(@event.BookId, @event.CreateUserId);
        _ = await _factoryHandler.Handler<CreateReaderCommand, Guid>(command, cancellationToken);
    }
}
