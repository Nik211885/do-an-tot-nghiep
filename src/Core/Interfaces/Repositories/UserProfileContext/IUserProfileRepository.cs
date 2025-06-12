using Core.BoundContext.UserProfileContext.UserProfileAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface IUserProfileRepository
    : IRepository<UserProfile>
{
    UserProfile Create(UserProfile userProfile);
    UserProfile Update(UserProfile userProfile);
    Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken= default);
}
