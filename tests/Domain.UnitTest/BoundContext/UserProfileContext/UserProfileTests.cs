using Core.BoundContext.UserProfileContext.UserProfileAggregate;

namespace Domain.UnitTest.BoundContext.UserProfileContext;

public class UserProfileTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var userId = Guid.NewGuid();
        var profile = UserProfile.Create(userId, "bio");
        Assert.Equal(userId, profile.Id);
        Assert.Equal("bio", profile.Bio);
    }

    [Fact]
    public void UpdateBio_Should_Change_Bio()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), "old");
        profile.UpdateBio("new");
        Assert.Equal("new", profile.Bio);
    }

    [Fact]
    public void AddCoutFollowing_Should_Increment()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), null);
        profile.AddCoutFollowing();
        Assert.Equal(1, profile.CountFollowing);
    }

    [Fact]
    public void UnCoutFollowing_Should_Decrement()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), null);
        profile.AddCoutFollowing();
        profile.UnCoutFollowing();
        Assert.Equal(0, profile.CountFollowing);
    }

    [Fact]
    public void AddFollower_Should_Increment()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), null);
        profile.AddFollower();
        Assert.Equal(1, profile.CoutFollowers);
    }

    [Fact]
    public void UnCoutFollower_Should_Decrement()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), null);
        profile.AddFollower();
        profile.UnCoutFollower();
        Assert.Equal(0, profile.CoutFollowers);
    }

    [Fact]
    public void AddCoutFavorite_Should_Increment()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), null);
        profile.AddCoutFavorite();
        Assert.Equal(1, profile.CountFavoriteBook);
    }

    [Fact]
    public void UnCoutFavorite_Should_Decrement()
    {
        var profile = UserProfile.Create(Guid.NewGuid(), null);
        profile.AddCoutFavorite();
        profile.UnCoutFavorite();
        Assert.Equal(0, profile.CountFavoriteBook);
    }
}
