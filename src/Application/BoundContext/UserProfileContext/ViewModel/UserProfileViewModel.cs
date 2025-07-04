using Core.BoundContext.UserProfileContext.UserProfileAggregate;

namespace Application.BoundContext.UserProfileContext.ViewModel;

public class UserProfileViewModel
{
    public Guid Id { get;  }
    public string? Bio { get; }
    public int CoutFollowing { get; }
    public int CoutFollower { get; }
    public int CoutFavoriteBook { get; }

    public UserProfileViewModel(Guid id, string? bio, int coutFollowing, int coutFollower, int coutFavoriteBook)
    {
        Id = id;
        Bio = bio;
        CoutFollowing = coutFollowing;
        CoutFollower = coutFollower;
        CoutFavoriteBook = coutFavoriteBook;
    }
}

public  static class MappingUserProfileExtension
{
    public static UserProfileViewModel ToViewModel(this UserProfile userProfile)
    {
        return new UserProfileViewModel(
            id: userProfile.Id,
            bio: userProfile.Bio,
            coutFollowing: userProfile.CountFollowing,
            coutFollower: userProfile.CoutFollowers,
            coutFavoriteBook: userProfile.CountFavoriteBook
        );
    }
}
