using Core.BoundContext.BookReviewContext.BookReviewAggregate;

namespace Application.BoundContext.BookReviewContext.ViewModel;

public class BookReviewViewModel
{
    public Guid Id { get; }
    public Guid BookId { get; }
    public int ViewCount { get; }
    public int CommentCount { get; }
    public int RatingCount { get; }
    public double AverageRating { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }

    public BookReviewViewModel(Guid id,Guid bookId, int viewCount, int commentCount, int ratingCount, double averageRating, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        Id = id;
        BookId = bookId;
        ViewCount = viewCount;
        CommentCount = commentCount;
        RatingCount = ratingCount;
        AverageRating = averageRating;
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
            averageRating: (double)bookReview.TotalRating / bookReview.RatingCount,
            createdAt: bookReview.CreatedAt,
            updatedAt: bookReview.LastUpdated
        );
    }
}
