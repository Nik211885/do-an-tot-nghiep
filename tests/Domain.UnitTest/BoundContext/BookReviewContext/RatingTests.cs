using Core.BoundContext.BookReviewContext.RatingAggregate;

namespace Domain.UnitTest.BoundContext.BookReviewContext;

public class RatingTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var reviewId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var rating = Rating.Create(reviewId, userId, 5);
        Assert.Equal(reviewId, rating.BookReviewId);
        Assert.Equal(userId, rating.ReviewerId);
        Assert.Equal(5, rating.Star.Star);
        Assert.NotNull(rating.DomainEvents);
        Assert.Single(rating.DomainEvents);
    }

    [Fact]
    public void UpdateRating_Should_Change_Star_And_Set_LastUpdated()
    {
        var rating = Rating.Create(Guid.NewGuid(), Guid.NewGuid(), 3);
        rating.UpdateRating(4);
        Assert.Equal(4, rating.Star.Star);
        Assert.NotNull(rating.LastUpdated);
        Assert.True(rating.LastUpdated > rating.DateTimeSubmitted);
        Assert.NotNull(rating.DomainEvents);
        Assert.True(rating.DomainEvents.Count >= 2); // Created + Updated
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Create_Should_Throw_When_Star_Invalid(int invalidStar)
    {
        var reviewId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() => Rating.Create(reviewId, userId, invalidStar));
        Assert.Contains("RatingStarNotFormat", ex.Message);
    }
}
