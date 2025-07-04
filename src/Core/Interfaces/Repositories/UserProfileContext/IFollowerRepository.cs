using Core.BoundContext.UserProfileContext.FollowerAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface IFollowerRepository
    : IRepository<Follower>
{
    Follower CreateFollower(Follower follower);
    void DeleteFollower(Follower follower);
    Task<Follower?> FindByFollowerAndFollowingAsync(Guid followerId, Guid followingId, CancellationToken cancellationToken = default);
}
