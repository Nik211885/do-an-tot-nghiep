using Core.BoundContext.UserProfileContext.FollowerAggregate;

namespace Domain.UnitTest.BoundContext.UserProfileContext;

public class FollowerTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var followingId = Guid.NewGuid();
        var followerId = Guid.NewGuid();
        var follower = Follower.Create(followingId, followerId);
        Assert.Equal(followingId, follower.FollowingId);
        Assert.Equal(followerId, follower.FollowerId);
        Assert.True(follower.FollowDate <= DateTimeOffset.UtcNow);
        Assert.NotNull(follower.DomainEvents);
        Assert.Single(follower.DomainEvents);
    }

    [Fact]
    public void UnFollow_Should_Raise_Event()
    {
        var follower = Follower.Create(Guid.NewGuid(), Guid.NewGuid());
        follower.UnFollow();
        Assert.NotNull(follower.DomainEvents);
        Assert.True(follower.DomainEvents.Count >= 2); // Followed + UnFollowed
    }
}
