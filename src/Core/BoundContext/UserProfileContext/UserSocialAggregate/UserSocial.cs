using Core.Entities;
using Core.Events.UserProfileContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.UserProfileContext.UserSocialAggregate;

public class UserSocial : BaseEntity,
    IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string Bio { get; private set; }
    private List<Follower> _followers;
    public IReadOnlyList<Follower> Followers => _followers.AsReadOnly();

    private UserSocial(Guid userId, string bio)
    {
        UserId = userId;
        Bio = bio;
    }

    public static UserSocial Create(Guid userId)
    {
        return new UserSocial(userId);
    }

    public void AddFollower(Guid followerId)
    {
        if (followerId == UserId)
        {
            throw new BadRequestException(UserProfileContextMessage.YouCanNotFollowYourself);
        }
        var followersExits = _followers.Where(f => f.FollowerId == followerId);
        if (followersExits.Any())
        {
            throw new BadRequestException(UserProfileContextMessage.YouHasFollowedUser);
        }
        var follower = Follower.Create(followerId);
        _followers.Add(follower);
        RaiseDomainEvent(new UserFollowedDomainEvent(UserId, followerId));
    }

    public void RemoveFollower(Guid followerId)
    {
        var followerExits = _followers.FirstOrDefault(f => f.FollowerId == followerId);
        if (followerExits == null)
        {
            throw new BadRequestException(UserProfileContextMessage.YouDontHasFollowedUser);
        }

        _followers.Remove(followerExits);
    }
}
