using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.UserProfileAggregate;

public class Follower 
{
    public Guid FollowerId { get; private set; }
    public DateTimeOffset FollowDate { get; private set; }
    protected Follower(){}
    private Follower(Guid followingId, Guid followerId)
    {
        FollowerId = followerId;
        FollowDate = DateTimeOffset.UtcNow;
    }
    
    public static Follower Create(Guid followingId, Guid followerId)
    {
        return new Follower(followingId,followerId);
    }
}
