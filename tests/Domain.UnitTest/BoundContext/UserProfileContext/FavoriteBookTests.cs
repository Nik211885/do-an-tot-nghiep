using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

namespace Domain.UnitTest.BoundContext.UserProfileContext;

public class FavoriteBookTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var fav = FavoriteBook.Create(userId, bookId);
        Assert.Equal(userId, fav.UserId);
        Assert.Equal(bookId, fav.FavoriteBookId);
        Assert.True(fav.CreatedOn <= DateTime.UtcNow);
        Assert.NotNull(fav.DomainEvents);
        Assert.Single(fav.DomainEvents);
    }

    [Fact]
    public void UnFavorite_Should_Raise_Event()
    {
        var fav = FavoriteBook.Create(Guid.NewGuid(), Guid.NewGuid());
        fav.UnFavorite();
        Assert.NotNull(fav.DomainEvents);
        Assert.True(fav.DomainEvents.Count >= 2); // Favored + UnFavored
    }
}
