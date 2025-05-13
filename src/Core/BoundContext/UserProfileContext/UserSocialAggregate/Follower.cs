using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.UserSocialAggregate;

public class Follower : ValueObject
{
    public Guid FollowerId { get; private set; }
    public DateTimeOffset FollowDate { get; private set; }

    private Follower(Guid followerId)
    {
        FollowerId = followerId;
        FollowDate = DateTimeOffset.UtcNow;
    }

    public static Follower Create(Guid followerId)
    {
        return new Follower(followerId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FollowerId;
        yield return FollowDate;
    }
}
