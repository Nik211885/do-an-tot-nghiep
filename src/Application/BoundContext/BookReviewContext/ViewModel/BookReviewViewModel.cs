using Core.BoundContext.BookReviewContext.BookReviewAggregate;

namespace Application.BoundContext.BookReviewContext.ViewModel;

public class BookReviewViewModel
{
    public Guid Id { get; }
    public Guid BookId { get; }
    public int ViewCount { get; }
    public int CommentCount { get; }
    public int RatingCount { get; }
    public long RatingStar { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }

    public BookReviewViewModel(Guid id,Guid bookId,
        int viewCount, int commentCount, int ratingCount,
        long ratingStar, DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        Id = id;
        BookId = bookId;
        ViewCount = viewCount;
        CommentCount = commentCount;
        RatingCount = ratingCount;
        RatingStar = ratingStar;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}

public static class BookReviewMappingExtension
{
    public static BookReviewViewModel MapToViewModel(this BookReview bookReview)
    {
        return new BookReviewViewModel(
            id: bookReview.Id,
            bookId:  bookReview.BookId,
            viewCount: bookReview.ViewCount,
            commentCount: bookReview.CommentCount,
            ratingCount: bookReview.RatingCount,
            ratingStar: bookReview.TotalRating,
            createdAt: bookReview.CreatedAt,
            updatedAt: bookReview.LastUpdated
        );
    }
}
