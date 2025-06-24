using Application.BoundContext.UserProfileContext.Message;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Application.Models;
using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.Favorite;

public record CreateFavoriteBookCommand(Guid BookId)
    : IUserProfileCommand<FavoriteBookViewModel>;

public class CreateFavoriteBookCommandHandler(
    ILogger<CreateFavoriteBookCommandHandler> logger,
    IFavoriteBookRepository favoriteBookRepository,
    IIdentityProvider identityProvider,
    IElasticFactory elasticFactory)
    : ICommandHandler<CreateFavoriteBookCommand, FavoriteBookViewModel>
{
    private readonly ILogger<CreateFavoriteBookCommandHandler> _logger = logger;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    private readonly IFavoriteBookRepository _favoriteBookRepository = favoriteBookRepository;
    public async Task<FavoriteBookViewModel> Handle(CreateFavoriteBookCommand request, CancellationToken cancellationToken)
    {
        var elasticFoBook = _elasticFactory.GetInstance<BookElasticModel>();
        var bookInElastic = await elasticFoBook.GetAsync(request.BookId);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookInElastic, "Không tìm thấy sách");
        /*if (!bookInElastic.IsActive)
        {
            ThrowHelper.ThrowNotFound("Sách");
        }*/
        // check book has active and has exits in elastic services
        var favoriteBookExits = await favoriteBookRepository
            .FindByBookAndUserAsync(_identityProvider.UserIdentity(), request.BookId, cancellationToken);
        ThrowHelper.ThrowBadRequestWhenITemIsNotNull(favoriteBookExits, FavoriteBookValidationMessage
            .YouCanNotFavoriteBookDuplicate);
        var favorite = FavoriteBook.Create(_identityProvider.UserIdentity(), request.BookId);
        _favoriteBookRepository.Create(favorite);
        await _favoriteBookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return favorite.ToViewModel();
    }
}

