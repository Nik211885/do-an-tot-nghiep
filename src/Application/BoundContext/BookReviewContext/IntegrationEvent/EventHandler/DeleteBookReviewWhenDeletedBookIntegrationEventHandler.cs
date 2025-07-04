using Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;
using Application.Exceptions;
using Application.Interfaces.EventBus;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.IntegrationEvent.EventHandler;

public class DeleteBookReviewWhenDeletedBookIntegrationEventHandler(
    IBookReviewRepository bookReviewRepository,
    ILogger<DeleteBookReviewWhenDeletedBookIntegrationEventHandler> logger)
    : IIntegrationEventHandler<DeletedBookIntegrationEvent>
{
    private readonly ILogger<DeleteBookReviewWhenDeletedBookIntegrationEventHandler> _logger = logger;

    public async Task Handle(DeletedBookIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        var bookReview = await bookReviewRepository.GetBookReviewByIdAsync(@event.BookId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookReview, "BookReview");
        bookReviewRepository.Delete(bookReview);
        await bookReviewRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
