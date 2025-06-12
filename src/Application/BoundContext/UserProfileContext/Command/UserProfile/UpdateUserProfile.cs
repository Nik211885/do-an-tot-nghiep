using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.UserProfile;

public record UpdateUserProfileCommand(Guid Id, string Bio)
    : IUserProfileCommand<UserProfileViewModel>;

public class UpdateUserProfileCommandHandler(
    ILogger<UpdateUserProfileCommandHandler> logger,
    IUserProfileRepository userProfileRepository)
    : ICommandHandler<UpdateUserProfileCommand, UserProfileViewModel>
{
    private readonly ILogger<UpdateUserProfileCommandHandler> _logger = logger;
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    public async Task<UserProfileViewModel> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _userProfileRepository.
            GetByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userProfile, "Thông tin cá nhân");
        userProfile.UpdateBio(request.Bio);
        _userProfileRepository.Update(userProfile);
        _logger.LogInformation("Update bio for user {@id}", userProfile.Id);
        await _userProfileRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return userProfile.ToViewModel();
    }
}
