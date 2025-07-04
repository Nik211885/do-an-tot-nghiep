using Core.BoundContext.BookReviewContext.RatingAggregate;

namespace Application.BoundContext.BookReviewContext.ViewModel;


public class RatingViewModel
{
    public Guid Id { get; }
    public Guid BookReviewId { get;  }
    public Guid? BookId { get;  }
    public Guid ReviewerId { get; }
    public int Star { get;  }
    public DateTimeOffset DateTimeSubmitted { get; }
    public DateTimeOffset? LastUpdated { get; }

    public RatingViewModel(Guid id, Guid bookReviewId, Guid reviewerId, int star, DateTimeOffset dateTimeSubmitted, DateTimeOffset? lastUpdated,
        Guid? bookId = null)
    {
        Id = id;
        BookId = bookId;
        BookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        Star = star;
        DateTimeSubmitted = dateTimeSubmitted;
        LastUpdated = lastUpdated;
    }
}

public static class RatingMappingExtension
{
    public static RatingViewModel MapToViewModel(this Rating rating, Guid? bookId = null)
    {
        return new RatingViewModel(
            id: rating.Id,
            bookId: bookId,
            bookReviewId:  rating.BookReviewId,
            reviewerId:  rating.ReviewerId,
            star: rating.Star.Star,
            dateTimeSubmitted: rating.DateTimeSubmitted,
            lastUpdated: rating.LastUpdated
        );
    }

    public static IReadOnlyCollection<RatingViewModel> MapToViewModel(this IEnumerable<Rating> ratings)
    {
        return ratings.Select(x=>x.MapToViewModel()).ToList();
    }
}
