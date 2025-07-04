using Core.BoundContext.BookReviewContext.RatingAggregate;

namespace Core.Interfaces.Repositories.BookReviewContext;

public interface IRatingRepository
    : IRepository<Rating>
{
    Rating CreateRating(Rating rating);
    Rating UpdateRating(Rating rating);
    Task<Rating?> GetRatingByIdAsync(Guid id, CancellationToken cancellationToken);
}
