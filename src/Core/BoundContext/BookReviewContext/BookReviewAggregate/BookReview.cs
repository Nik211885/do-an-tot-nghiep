using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class BookReview : BaseEntity, IAggregateRoot
{
    public Guid BookId { get; private set; }
    public int ViewCount { get; private set; }
    public int CommentCount { get; private set; }
    public int RatingCount { get; private set; }
    public long TotalRating { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset LastUpdated { get; private set; }
    protected BookReview(){}
    private BookReview(Guid bookId)
    {
        BookId = bookId;
        ViewCount = 0;
        CommentCount = 0;
        RatingCount = 0;
        TotalRating = 0; 
        CreatedAt = DateTimeOffset.UtcNow;
        LastUpdated = DateTimeOffset.UtcNow;
    }

    public static BookReview Create(Guid bookId)
    {
        var bookReview = new BookReview(bookId);
        bookReview.RaiseDomainEvent(new BookReviewCreatedDomainEvent(bookReview.Id, bookId));
        return bookReview;
    }

    public void IncrementView()
    {
        ViewCount += 1;
    }
    public void IncrementRating(int star)
    {
        TotalRating += star;
        RatingCount+=1;
    }

    public void UnIncrementRating(int newStarValue, int oldStarValue)
    {
        TotalRating -= newStarValue;
        TotalRating += oldStarValue;
    }

    public void UpdateCommentCount()
    {
        CommentCount+=1;
    }
}
