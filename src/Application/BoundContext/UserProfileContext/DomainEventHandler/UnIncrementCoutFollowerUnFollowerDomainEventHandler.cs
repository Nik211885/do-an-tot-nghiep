using Application.BoundContext.UserProfileContext.Command;
using Application.BoundContext.UserProfileContext.Message;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Events.UserProfileContext;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.DomainEventHandler;

public class UnIncrementCoutFollowerUnFollowerDomainEventHandler(
    ILogger<UnIncrementCoutFollowerUnFollowerDomainEventHandler> logger,
    IUserProfileRepository userProfileRepository)
    : IEventHandler<UnFollowedDomainEvent>
{
    private readonly ILogger<UnIncrementCoutFollowerUnFollowerDomainEventHandler> _logger = logger;
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    public async Task Handler(UnFollowedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var userFollower =
            await _userProfileRepository.GetByIdAsync(domainEvent.UnFollowed.FollowerId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userFollower,"Người theo dõi");
        var userFollowing = 
            await _userProfileRepository.GetByIdAsync(domainEvent
                .UnFollowed.FollowingId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userFollowing, "Người được theo dõi");
        userFollower.UnCoutFollower();
        userFollowing.UnCoutFollowing();
        _userProfileRepository.Update(userFollower);
        _userProfileRepository.Update(userFollowing);
        await _userProfileRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
