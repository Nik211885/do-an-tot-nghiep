using Application.BoundContext.UserProfileContext.Queries;
using Application.BoundContext.UserProfileContext.ViewModel;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class UserProfileQueries(UserProfileDbContext userProfileDbContext) : IUserProfileQueries
{
    private readonly UserProfileDbContext _userProfileDbContext = userProfileDbContext;

    public async Task<UserProfileViewModel?> GetUserProfileByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userProfile = await _userProfileDbContext
            .UserProfiles
            .AsNoTracking()
            .Where(x=>x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return userProfile?.ToViewModel();
    }
}
