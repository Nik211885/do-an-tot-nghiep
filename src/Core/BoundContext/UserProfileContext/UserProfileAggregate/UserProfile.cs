using Core.Entities;
using Core.Events.UserProfileContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.UserProfileContext.UserProfileAggregate;

public class UserProfile : BaseEntity, IAggregateRoot
{
    public sealed override Guid Id { get; protected set; }
    public string? Bio { get; private set; }
    public int CountFollowing { get; private set; }
    public int CoutFollowers { get; private set; }
    public int CountFavoriteBook { get; private set; }
    protected UserProfile(){}
    private UserProfile(Guid userId, string? bio)
    {
        Id  = userId;
        Bio = bio;
    }
    public void UpdateBio(string bio)
    {
        Bio = bio;
    }
    public static UserProfile Create(Guid userId, string? bio)
    {
        return new UserProfile(userId, bio);
    }

    public void AddCoutFollowing()
    {
        CountFollowing += 1;
    }

    public void UnCoutFollowing()
    {
        CountFollowing -= 1;
    }

    public void AddFollower()
    {
        CoutFollowers += 1;
    }

    public void UnCoutFollower()
    {
        CoutFollowers -= 1;
    }

    public void AddCoutFavorite()
    {
        CountFavoriteBook += 1;
    }

    public void UnCoutFavorite()
    {
        CountFavoriteBook -= 1;
    }
        
}
