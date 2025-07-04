using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Events.UserProfileContext;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.DomainEventHandler;

public class IncrementCoutFollowerUserFollowedDomainEventHandler(
    IUserProfileRepository userProfileRepository,
    ILogger<IncrementCoutFollowerUserFollowedDomainEventHandler> logger)
    : IEventHandler<UserFollowedDomainEvent>
{
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    private readonly ILogger<IncrementCoutFollowerUserFollowedDomainEventHandler> _logger = logger;
    public async Task Handler(UserFollowedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var userFollower = await _userProfileRepository.GetByIdAsync(domainEvent.Followed.FollowingId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userFollower,"Người theo dõi");
        var userFollowing = await _userProfileRepository.GetByIdAsync(domainEvent.Followed.FollowingId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userFollowing, "Người được theo dõi");
        userFollower.AddFollower();
        userFollowing.AddCoutFollowing();
        _userProfileRepository.Update(userFollower);
        _userProfileRepository.Update(userFollowing);
        await _userProfileRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
