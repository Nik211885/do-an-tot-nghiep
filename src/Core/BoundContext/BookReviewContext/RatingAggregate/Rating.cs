using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Interfaces;

namespace Core.BoundContext.BookReviewContext.RatingAggregate;

public class Rating : BaseEntity, IAggregateRoot
{
    public Guid BookReviewId { get; private set; }
    public Guid ReviewerId { get; private set; }
    public RatingStar Star { get; private set; }
    public DateTimeOffset DateTimeSubmitted { get; private set; }
    public DateTimeOffset? LastUpdated { get; private set; }
    
    protected Rating() { }
    
    private Rating(Guid bookReviewId, Guid reviewerId, RatingStar star)
    {
        BookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        Star = star;
        DateTimeSubmitted = DateTimeOffset.UtcNow;
    }

    public static Rating Create(Guid bookReviewId, Guid reviewerId, int starValue)
    {
        var star = RatingStar.Create(starValue);
        var rating = new Rating(bookReviewId, reviewerId, star);
        
        rating.RaiseDomainEvent(new RatingCreatedDomainEvent(
            rating.Id, bookReviewId, reviewerId, starValue));
            
        return rating;
    }

    public void UpdateRating(int newStarValue)
    {
        var oldStarValue = Star.Star;
        Star = RatingStar.Create(newStarValue);
        LastUpdated = DateTimeOffset.UtcNow;
        
        RaiseDomainEvent(new RatingUpdatedDomainEvent(
            Id, BookReviewId, ReviewerId, oldStarValue, newStarValue));
    }
}
