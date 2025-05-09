using Core.Entities;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class Rating : BaseEntity
{
    private Guid _bookReviewId;
    public Guid ReviewerId { get; private set; }
    public RatingStar Stars { get; private set; }
    
    public DateTimeOffset DateTimeReviewSubmit { get; private set; }

    private Rating(Guid bookReviewId, Guid reviewerId, RatingStar stars)
    {
        ReviewerId = reviewerId;
        Stars = stars;
        _bookReviewId = bookReviewId;
        DateTimeReviewSubmit = DateTimeOffset.UtcNow;
    }

    public static Rating Create(Guid bookReviewId, Guid reviewerId, RatingStar stars)
    {
        return new Rating(bookReviewId, reviewerId, stars);
    }

    public void Update(RatingStar stars)
    {
        Stars = stars;
        DateTimeReviewSubmit = DateTimeOffset.UtcNow;
    }
    
}
