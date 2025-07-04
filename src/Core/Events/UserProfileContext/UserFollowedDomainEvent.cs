

using Core.BoundContext.UserProfileContext.FollowerAggregate;

namespace Core.Events.UserProfileContext;

public class UserFollowedDomainEvent(Follower follower) : IEvent
{
    public Follower Followed { get; } = follower;
}
