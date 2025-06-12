using Core.BoundContext.UserProfileContext.FollowerAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface IFollowerRepository
    : IRepository<Follower>
{
    Follower CreateFollower(Follower follower);
    void DeleteFollower(Follower follower);
}
