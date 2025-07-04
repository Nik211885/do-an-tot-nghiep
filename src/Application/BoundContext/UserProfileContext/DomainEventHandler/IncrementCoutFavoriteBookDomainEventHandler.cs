using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Events.UserProfileContext;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.DomainEventHandler;

public class IncrementCoutFavoriteBookDomainEventHandler(
    ILogger<IncrementCoutFavoriteBookDomainEventHandler> logger,
    IUserProfileRepository userProfileRepository,
    IIdentityProvider identityProvider)
    : IEventHandler<FavoredBookDomainEvent>
{
    private readonly ILogger<IncrementCoutFavoriteBookDomainEventHandler> _logger = logger;
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    public async Task Handler(FavoredBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var userProfile =
            await _userProfileRepository.GetByIdAsync(domainEvent.FavoriteBook.UserId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userProfile, "Thông tin người dùng");
        
        userProfile.AddCoutFavorite();
        _userProfileRepository.Update(userProfile);
        await _userProfileRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
