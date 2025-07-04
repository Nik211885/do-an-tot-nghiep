using Core.BoundContext.UserProfileContext.UserProfileAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.UserProfileContext;

public class UserProfileRepository(UserProfileDbContext dbContext) 
    : Repository<UserProfile>(dbContext), IUserProfileRepository
{
    private readonly UserProfileDbContext _userProfileDbContext = dbContext;
    public UserProfile Create(UserProfile userProfile)
    {
        return _userProfileDbContext.UserProfiles.Add(userProfile).Entity;
    }

    public UserProfile Update(UserProfile userProfile)
    {
        return _userProfileDbContext.UserProfiles.Update(userProfile).Entity;
    }

    public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userProfile = await _userProfileDbContext
            .UserProfiles
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        return userProfile;
    }
}
