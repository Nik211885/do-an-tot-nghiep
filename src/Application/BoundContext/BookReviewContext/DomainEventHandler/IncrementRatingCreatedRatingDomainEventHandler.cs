using Application.Interfaces.CQRS;
using Core.Events.BookReviewContext;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.DomainEventHandler;

public class IncrementRatingCreatedRatingDomainEventHandler(
    ILogger<IncrementRatingCreatedRatingDomainEventHandler> logger,
    IBookReviewRepository bookReviewRepository)
    : IEventHandler<RatingCreatedDomainEvent>
{
    private readonly ILogger<IncrementRatingCreatedRatingDomainEventHandler> _logger = logger;
    private readonly IBookReviewRepository _bookReviewRepository = bookReviewRepository;
    public async Task Handler(RatingCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookReview = await _bookReviewRepository.GetBookReviewByIdAsync(domainEvent.BookReviewId, cancellationToken);
        if (bookReview is null)
        {
            return;
        }
        bookReview.IncrementRating(domainEvent.StarValue);
        _bookReviewRepository.UpdateBookReview(bookReview);
        await _bookReviewRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
