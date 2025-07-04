using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.BoundContext.UserProfileContext.IntegrationEvent.Event;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.IntegrationEvent.EventHandler;

public class CreatedUserUpdateProfileIntegrationEventHandler(
    ILogger<CreatedUserUpdateProfileIntegrationEventHandler> logger,
    IFactoryHandler factoryHandler)
    : IIntegrationEventHandler<KeycloakUserCreatedIntegrationEvent>
{
    private readonly ILogger<CreatedUserUpdateProfileIntegrationEventHandler> _logger = logger;
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    public async Task Handle(KeycloakUserCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Processing Keycloak user created event for user: {UserId}", @event.UserId);
        var userProfile = new CreateUserProfileCommand(Guid.Parse(@event.UserId), null);
        await _factoryHandler.Handler<CreateUserProfileCommand, UserProfileViewModel>(userProfile, cancellationToken);
        _logger.LogInformation("Created user has success {@Id}", @event.UserId);
    }
}   
