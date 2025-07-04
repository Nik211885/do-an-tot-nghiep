using Application.BoundContext.BookReviewContext.Command.BookReview;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.IntegrationEvent.EventHandler;

public class CreatedBookReviewWhenApprovedBookIntegrationEventHandler(
    ILogger<CreatedBookReviewWhenApprovedBookIntegrationEventHandler> logger,
    IFactoryHandler factoryHandler)
    : IIntegrationEventHandler<ApprovalBookIntegrationEvent>
{
    private readonly ILogger<CreatedBookReviewWhenApprovedBookIntegrationEventHandler> _logger = logger;
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    public async Task Handle(ApprovalBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var command = new CreateBookReviewCommand(@event.BookId);
        await _factoryHandler.Handler<CreateBookReviewCommand, BookReviewViewModel>(command, cancellationToken);
    }
}
