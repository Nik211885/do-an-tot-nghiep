using Core.BoundContext.UserProfileContext.FollowerAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.UserProfileContext;

public class FollowerRepository(UserProfileDbContext dbContext) 
    : Repository<Follower>(dbContext), IFollowerRepository
{
    private readonly UserProfileDbContext _userProfileDbContext = dbContext;
    public Follower CreateFollower(Follower follower)
    {
        return _userProfileDbContext.Followers.Add(follower).Entity;
    }

    public void DeleteFollower(Follower follower)
    {
        _userProfileDbContext.Followers.Remove(follower);
    }
}
