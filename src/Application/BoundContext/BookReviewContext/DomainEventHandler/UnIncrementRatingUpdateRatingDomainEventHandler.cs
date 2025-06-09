using Application.Interfaces.CQRS;
using Core.Events.BookReviewContext;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.DomainEventHandler;

public class UnIncrementRatingUpdateRatingDomainEventHandler(ILogger<IncrementRatingCreatedRatingDomainEventHandler> logger,
    IBookReviewRepository bookReviewRepository)
    : IEventHandler<RatingUpdatedDomainEvent>
{
    private readonly ILogger<IncrementRatingCreatedRatingDomainEventHandler> _logger = logger;
    private readonly IBookReviewRepository _bookReviewRepository = bookReviewRepository;
    public async Task Handler(RatingUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookReview = await _bookReviewRepository.GetBookReviewByIdAsync(domainEvent.BookReviewId, cancellationToken);
        if (bookReview is null)
        {
            return;
        }
        bookReview.UnIncrementRating(domainEvent.NewStarValue, domainEvent.OldStarValue);
        _bookReviewRepository.UpdateBookReview(bookReview);
        await _bookReviewRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
