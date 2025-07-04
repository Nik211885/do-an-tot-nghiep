

using Core.BoundContext.UserProfileContext.FollowerAggregate;

namespace Core.Events.UserProfileContext;

public class UnFollowedDomainEvent(Follower follower) : IEvent
{
    public Follower UnFollowed { get; } = follower;
}
