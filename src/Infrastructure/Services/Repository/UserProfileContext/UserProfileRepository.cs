using Core.BoundContext.UserProfileContext.UserProfileAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;

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
}
