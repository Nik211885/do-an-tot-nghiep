using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.Query;

namespace Application.BoundContext.UserProfileContext.Queries;

public interface IUserProfileQueries : IApplicationQueryServicesExtension
{
    // user
    Task<UserProfileViewModel?> GetUserProfileByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
