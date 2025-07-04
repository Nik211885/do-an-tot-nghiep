namespace Core.Events.BookReviewContext;

public class RatingUpdatedDomainEvent : IEvent
{
    public Guid Id { get; }
    public Guid BookReviewId { get;  }
    public Guid  ReviewerId { get;  }
    public int OldStarValue { get;  }
    public int NewStarValue { get;  }

    public RatingUpdatedDomainEvent(Guid id, Guid bookReviewId, Guid reviewerId, int oldStarValue, int newStarValue)
    {
        Id = id;
        BookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        OldStarValue = oldStarValue;
        NewStarValue = newStarValue;
    }
}
