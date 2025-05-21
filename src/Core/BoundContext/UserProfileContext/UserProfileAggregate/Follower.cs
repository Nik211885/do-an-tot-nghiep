using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.UserProfileAggregate;

public class Follower : ValueObject
{
    public Guid FollowingId { get; private set; }
    public Guid FollowerId { get; private set; }
    public DateTimeOffset FollowDate { get; private set; }
    protected Follower(){}
    private Follower(Guid followingId, Guid followerId)
    {
        FollowingId = followingId;
        FollowerId = followerId;
        FollowDate = DateTimeOffset.UtcNow;
    }
    
    public static Follower Create(Guid followingId, Guid followerId)
    {
        return new Follower(followingId,followerId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FollowerId;
        yield return FollowingId;
        yield return FollowDate;
    }
}
