namespace Core.Events.BookReviewContext;

public class RatingCreatedDomainEvent : IEvent
{
    public Guid Id { get;  }
    public Guid BookReviewId { get;  }
    public Guid ReviewerId { get;  }
    public int StarValue { get; }

    public RatingCreatedDomainEvent(Guid id, Guid bookReviewId, Guid reviewerId, int starValue)
    {
        Id = id;
        BookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        StarValue = starValue;
    }
}
