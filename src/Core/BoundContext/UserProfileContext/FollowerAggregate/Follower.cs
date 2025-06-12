using Core.Entities;
using Core.Events.UserProfileContext;
using Core.Interfaces;

namespace Core.BoundContext.UserProfileContext.FollowerAggregate;

public class Follower : BaseEntity,IAggregateRoot
{
    public Guid FollowingId { get; set; }
    public Guid FollowerId { get; private set; }
    public DateTimeOffset FollowDate { get; private set; }
    protected Follower(){}
    private Follower(Guid followingId, Guid followerId)
    {
        FollowerId = followerId;
        FollowingId = followingId;
        FollowDate = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new UserFollowedDomainEvent(this));
    }
    
    public static Follower Create(Guid followingId, Guid followerId)
    {
        return new Follower(followingId,followerId);
    }
    
    public void UnFollow()
    {
        RaiseDomainEvent(new UnFollowedDomainEvent(this));
    }
}
