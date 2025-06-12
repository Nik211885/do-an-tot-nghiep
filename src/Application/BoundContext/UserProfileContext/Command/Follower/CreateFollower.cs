using Application.BoundContext.UserProfileContext.Message;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.Follower;

public record CreateFollowerCommand(Guid FollowingId)
    : IUserProfileCommand<FollowerViewModel>;

public class CreateFollowerCommandHandler(
    ILogger<CreateFollowerCommandHandler> logger,
    IValidationServices<Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile> userProfileValidation,
    IFollowerRepository followerRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<CreateFollowerCommand, FollowerViewModel>
{
    private readonly ILogger<CreateFollowerCommandHandler> _logger = logger;
    private readonly IValidationServices<Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile> _userProfileValidation = userProfileValidation;
    private readonly IFollowerRepository _followerRepository = followerRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<FollowerViewModel> Handle(CreateFollowerCommand request, CancellationToken cancellationToken)
    {
        if (request.FollowingId == _identityProvider.UserIdentity())
        {
            ThrowHelper.ThrowIfBadRequest(FollowerValidationMessage.CanNotFollowerYourSelf);
        }
        var userFollowerId = await _userProfileValidation
            .AnyAsync(x=>x.Id == request.FollowingId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userFollowerId, "Nguời cần theo dõi");
        // 
        var followerExits = await _followerRepository.FindByFollowerAndFollowingAsync(_identityProvider.UserIdentity(),
            request.FollowingId, cancellationToken);
        if (followerExits is not null)
        {
            ThrowHelper.ThrowIfBadRequest(FollowerValidationMessage.YouHasFollowerUser);
        }
        var follower = Core.BoundContext.UserProfileContext.FollowerAggregate.Follower.Create(
            request.FollowingId, _identityProvider.UserIdentity());
        _followerRepository.CreateFollower(follower);
        await _followerRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return follower.ToViewModel();
    }
}
