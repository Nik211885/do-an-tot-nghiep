using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.UserProfileContext;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.UserProfile;

public record CreateUserProfileCommand(Guid UserId, string? Bio) :
    IUserProfileCommand<UserProfileViewModel>;

public class CreateUserProfileCommandHandler(
    IUserProfileRepository repository,
    ILogger<CreateUserProfileCommandHandler> logger)
    : ICommandHandler<CreateUserProfileCommand, UserProfileViewModel>
{
    private readonly IUserProfileRepository _repository = repository;
    private readonly ILogger<CreateUserProfileCommandHandler> _logger = logger;
    public async Task<UserProfileViewModel> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = Core.BoundContext.UserProfileContext.UserProfileAggregate.UserProfile.Create(
                userId: request.UserId,
                bio: request.Bio
        );
        _repository.Create(userProfile);
        _logger.LogInformation("Sync data from user profile  has {@Id}", request.UserId);
        await _repository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Sync success data from iam profile has {@Id}", request.UserId);
        return userProfile.ToViewModel();
    }
}
