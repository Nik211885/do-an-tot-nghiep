using Core.BoundContext.BookReviewContext.BookReviewAggregate;

namespace Domain.UnitTest.BoundContext.BookReviewContext;

public class BookReviewTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var bookId = Guid.NewGuid();
        var review = BookReview.Create(bookId);
        Assert.Equal(bookId, review.BookId);
        Assert.Equal(0, review.ViewCount);
        Assert.Equal(0, review.CommentCount);
        Assert.Equal(0, review.RatingCount);
        Assert.Equal(0, review.TotalRating);
    }

    [Fact]
    public void IncrementView_Should_Increase_ViewCount()
    {
        var review = BookReview.Create(Guid.NewGuid());
        review.IncrementView();
        Assert.Equal(1, review.ViewCount);
    }

    [Fact]
    public void IncrementRating_Should_Increase_RatingCount_And_TotalRating()
    {
        var review = BookReview.Create(Guid.NewGuid());
        review.IncrementRating(4);
        Assert.Equal(1, review.RatingCount);
        Assert.Equal(4, review.TotalRating);
    }

    [Fact]
    public void UnIncrementRating_Should_Adjust_TotalRating()
    {
        var review = BookReview.Create(Guid.NewGuid());
        review.IncrementRating(5);
        review.UnIncrementRating(5, 3);
        Assert.Equal(3, review.TotalRating);
    }

    [Fact]
    public void UpdateCommentCount_Should_Increase_CommentCount()
    {
        var review = BookReview.Create(Guid.NewGuid());
        review.UpdateCommentCount();
        Assert.Equal(1, review.CommentCount);
    }
}
