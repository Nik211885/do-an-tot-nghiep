using Core.BoundContext.UserProfileContext.FollowerAggregate;

namespace Application.BoundContext.UserProfileContext.ViewModel;

public class FollowerViewModel
{   
    public Guid Id { get;  }
    public Guid FollowingId { get;  }
    public Guid FollowerId { get;  }
    public DateTimeOffset FollowDate { get;  }

    public FollowerViewModel(Guid id, Guid followingId, Guid followerId, DateTimeOffset followDate)
    {
        Id = id;
        FollowingId = followingId;
        FollowerId = followerId;
        FollowDate = followDate;
    }
}

public static class MappingFollowerExtensions
{
    public static FollowerViewModel ToViewModel(this Follower follower)
    {
        return new FollowerViewModel(
            id: follower.Id,
            followingId: follower.FollowingId,
            followerId: follower.FollowerId,
            followDate: follower.FollowDate
            );
    }

    public static IEnumerable<FollowerViewModel> ToViewModel(this IEnumerable<Follower> followers)
    {
        return followers.Select(ToViewModel);
    }
}
