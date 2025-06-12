using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.Follower;

public record DeleteFollowerCommand(Guid FollowingId)
    : IUserProfileCommand<FollowerViewModel>;

public class DeleteFollowerCommandHandler(
    ILogger<DeleteFollowerCommandHandler> logger,
    IFollowerRepository followerRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<DeleteFollowerCommand, FollowerViewModel>
{
    private readonly ILogger<DeleteFollowerCommandHandler> _logger = logger;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IFollowerRepository _followerRepository = followerRepository;

    public async Task<FollowerViewModel> Handle(DeleteFollowerCommand request, CancellationToken cancellationToken)
    {
        var follower = await _followerRepository.FindByFollowerAndFollowingAsync(_identityProvider.UserIdentity(), request.FollowingId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(follower, "Luợt theo dõi");
        follower.UnFollow();
        _followerRepository.DeleteFollower(follower);
        await _followerRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return follower.ToViewModel();
    }
}
