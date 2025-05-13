using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class Rating : ValueObject
{
    public Guid ReviewerId { get; private set; }
    public RatingStar Star { get; private set; }
    
    public DateTimeOffset DateTimeReviewSubmit { get; private set; }

    private Rating(Guid reviewerId, RatingStar star)
    {
        ReviewerId = reviewerId;
        Star = star;
        DateTimeReviewSubmit = DateTimeOffset.UtcNow;
    }

    public static Rating Create(Guid reviewerId, RatingStar star)
    {
        return new Rating(reviewerId, star);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ReviewerId;
        yield return Star;
        yield return DateTimeReviewSubmit;
    }
}
