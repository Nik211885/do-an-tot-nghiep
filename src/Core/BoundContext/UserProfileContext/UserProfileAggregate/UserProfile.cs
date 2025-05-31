using Core.Entities;
using Core.Events.UserProfileContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.UserProfileContext.UserProfileAggregate;

public class UserProfile : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public string? Bio { get; private set; }
    public int CountFollowing { get; private set; }
    public int CoutFollowers { get; private set; }
    private List<Follower> _followers;
    public int CountFavoriteBook { get; private set; }
    public IReadOnlyList<Follower> Followers => _followers.AsReadOnly();
    private List<FavoriteBook> _favoriteItems;
    public IReadOnlyCollection<FavoriteBook> FavoriteItems => _favoriteItems.AsReadOnly();
    protected UserProfile(){}
    private UserProfile(Guid userId, string bio)
    {
        UserId = userId;
        Bio = bio;
    }
    public void UpdateBio(string bio)
    {
        Bio = bio;
    }
    public static UserProfile Create(Guid userId, string bio)
    {
        return new UserProfile(userId, bio);
    }
    
    public void AddFollower(Guid followerId)
    {
        if (followerId == UserId)
        {
            ThrowHelper.ThrowIfBadRequest(UserProfileContextMessage.YouCanNotFollowYourself);
        }
        var followersExits = _followers.Where(f => f.FollowerId == followerId);
        if (followersExits.Any())
        {
            ThrowHelper.ThrowIfBadRequest(UserProfileContextMessage.YouHasFollowedUser);
        }
        var follower = Follower.Create(UserId,followerId);
        _followers.Add(follower);
        RaiseDomainEvent(new UserFollowedDomainEvent(UserId, followerId));
    }

    public void RemoveFollower(Guid followerId)
    {
        var followerExits = _followers.FirstOrDefault(f => f.FollowerId == followerId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(followerExits,UserProfileContextMessage.YouDontHasFollowedUser);
        _followers.Remove(followerExits);
    }
    public void FavoriteItem(Guid favoriteBookId)
    {
        var favoredItemExits = _favoriteItems.FirstOrDefault(x => x.FavoriteBookId == favoriteBookId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(favoredItemExits,UserProfileContextMessage.YouHasFavoriteItem);
        _favoriteItems.Add(FavoriteBook.Create(UserId,favoriteBookId));
        RaiseDomainEvent(new FavoredBookDomainEvent(UserId,favoriteBookId));
    }
}
