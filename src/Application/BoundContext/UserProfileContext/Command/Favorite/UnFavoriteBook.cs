using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.Favorite;

public record UnFavoriteBookCommand(Guid BookId)
    : IUserProfileCommand<FavoriteBookViewModel>;

public class UnFavoriteBookCommandHandler(
    ILogger<UnFavoriteBookCommandHandler> logger,
    IFavoriteBookRepository favoriteBookRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<UnFavoriteBookCommand, FavoriteBookViewModel>
{
    private readonly ILogger<UnFavoriteBookCommandHandler> _logger = logger;
    private readonly IFavoriteBookRepository _favoriteBookRepository = favoriteBookRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<FavoriteBookViewModel> Handle(UnFavoriteBookCommand request, CancellationToken cancellationToken)
    {
        var favorite = await _favoriteBookRepository
            .FindByBookAndUserAsync(_identityProvider.UserIdentity(), request.BookId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(favorite, "Lựợt yêu thích");
        favorite.UnFavorite();
        _favoriteBookRepository.Delete(favorite);
        await _favoriteBookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return favorite.ToViewModel();
    }
}
