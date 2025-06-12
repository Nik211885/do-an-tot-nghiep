using Core.BoundContext.UserProfileContext.FollowerAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Follower?> FindByFollowerAndFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken =default)
    {
        var follower = await _userProfileDbContext.Followers
            .Where(x => x.FollowerId == followerId && x.FollowingId == followingId)
            .FirstOrDefaultAsync(cancellationToken);
        return follower;
    }
}
