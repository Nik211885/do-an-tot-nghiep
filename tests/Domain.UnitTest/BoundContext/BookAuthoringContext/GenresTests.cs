using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Domain.UnitTest.BoundContext.BookAuthoringContext;

public class GenresTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var genres = Genres.Create(userId, "name", "desc", "slug", "avatar");
        Assert.Equal(userId, genres.CreateUserId);
        Assert.Equal("name", genres.Name);
        Assert.Equal("desc", genres.Description);
        Assert.Equal("slug", genres.Slug);
        Assert.Equal("avatar", genres.AvatarUrl);
        Assert.True(genres.IsActive);
        Assert.Equal(0, genres.CountBook);
    }

    [Fact]
    public void Update_Should_Change_Properties()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        genres.Update("newName", "newDesc", "newAvatar", "newSlug");
        Assert.Equal("newName", genres.Name);
        Assert.Equal("newDesc", genres.Description);
        Assert.Equal("newSlug", genres.Slug);
        Assert.Equal("newAvatar", genres.AvatarUrl);
    }

    [Fact]
    public void Activate_Should_Set_IsActive_True()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        genres.UnActivate();
        genres.Activate();
        Assert.True(genres.IsActive);
    }

    [Fact]
    public void UnActivate_Should_Set_IsActive_False()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        genres.UnActivate();
        Assert.False(genres.IsActive);
    }

    [Fact]
    public void UpdateAvatar_Should_Change_AvatarUrl()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        genres.UpdateAvatar("newAvatar");
        Assert.Equal("newAvatar", genres.AvatarUrl);
    }

    [Fact]
    public void AddCoutForBook_Should_Increment_CountBook()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        genres.AddCoutForBook();
        Assert.Equal(1, genres.CountBook);
    }

    [Fact]
    public void RemoveCoutForBook_Should_Decrement_CountBook()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        genres.AddCoutForBook();
        genres.RemoveCoutForBook();
        Assert.Equal(0, genres.CountBook);
    }

    [Fact]
    public void ChangeActive_Should_Toggle_IsActive()
    {
        var genres = Genres.Create(Guid.NewGuid(), "name", "desc", "slug", "avatar");
        var initial = genres.IsActive;
        genres.ChangeActive();
        Assert.NotEqual(initial, genres.IsActive);
    }
}
