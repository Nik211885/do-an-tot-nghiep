using Core.Entities;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class Rating : BaseEntity
{
    private Guid _bookReviewId;
    public Guid ReviewerId { get; private set; }
    public RatingStar Star { get; private set; }
    
    public DateTimeOffset DateTimeReviewSubmit { get; private set; }

    private Rating(Guid bookReviewId, Guid reviewerId, RatingStar star)
    {
        ReviewerId = reviewerId;
        Star = star;
        _bookReviewId = bookReviewId;
        DateTimeReviewSubmit = DateTimeOffset.UtcNow;
    }

    public static Rating Create(Guid bookReviewId, Guid reviewerId, RatingStar star)
    {
        return new Rating(bookReviewId, reviewerId, star);
    }

    public void Update(RatingStar stars)
    {
        Star = stars;
        DateTimeReviewSubmit = DateTimeOffset.UtcNow;
    }
    
}
