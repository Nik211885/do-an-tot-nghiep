using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Events.UserProfileContext;
using Core.Interfaces.Repositories.UserProfileContext;

namespace Application.BoundContext.UserProfileContext.DomainEventHandler;

public class UnIncrementCoutFavoriteBookDomainEventHandler(
    IIdentityProvider identityProvider,
    IUserProfileRepository userProfileRepository)
    : IEventHandler<UnFavoredBookDomainEvent>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;
    public async Task Handler(UnFavoredBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(domainEvent.UnFavoredBook.UserId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(userProfile, "Thông tin người dùng");
        userProfile.UnCoutFavorite();
        _userProfileRepository.Update(userProfile);
        await _userProfileRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
