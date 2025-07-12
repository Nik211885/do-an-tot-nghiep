using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.IntegrationEvent.EventHandler;

public class CreatedBookCreateNewBookApprovalIntegrationEventHandler(
    IFactoryHandler factoryHandler,
    ILogger<CreatedBookCreateNewBookApprovalIntegrationEventHandler> logger)
    : IIntegrationEventHandler<CreatedBookIntegrationEvent>
{
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    private readonly ILogger<CreatedBookCreateNewBookApprovalIntegrationEventHandler> _logger = logger;
    public async Task Handle(CreatedBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consumer with event {event}", @event);
        var createdBook = @event.Book;
        var createBookApprovalCommand =
            new CreateBookApprovalCommand(createdBook.Id, createdBook.Title, createdBook.CreatedUerId);
        _ = await _factoryHandler.Handler<CreateBookApprovalCommand, BookApprovalViewModel>(createBookApprovalCommand, cancellationToken);
    }
}
