using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;
using GenreCore = Core.BoundContext.BookAuthoringContext.GenresAggregate.Genres;

namespace Application.BoundContext.BookAuthoringContext.Command.Genre;
/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="Description"></param>
/// <param name="AvatarUrl"></param>
public record CreateGenreCommand(
    string Name,
    string Description,
    string AvatarUrl) : IBookAuthoringCommand<GenreViewModel>;

/// <summary>
/// 
/// </summary>
/// <param name="genresRepository"></param>
/// <param name="logger"></param>
/// <param name="identityProvider"></param>

public class CreateGenreCommandHandle(
    IGenresRepository genresRepository,
    ILogger<CreateBookCommandHandler> logger,
    ICache cache,
    IIdentityProvider identityProvider)
    : ICommandHandler<CreateGenreCommand, GenreViewModel>
{
    private readonly ICache _cache = cache;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly ILogger<CreateBookCommandHandler> _logger = logger;
    private readonly IGenresRepository _genresRepository = genresRepository;
    public async Task<GenreViewModel> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = GenreCore.Create(
            createUserId: _identityProvider.UserIdentity(),
            name: request.Name,
            description: request.Description,
            avatarUrl: request.AvatarUrl,
            slug: request.Name.CreateSlug()
        );
        _logger.LogInformation("Created genre pass domain rule for userId {userId} has created",
            _identityProvider.UserIdentity());
        var genres = _genresRepository.Create(genre);
        await _genresRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Create genre success with id is {genreIdd} and user created is {userId}",
            genre.Id, _identityProvider.UserIdentity());
        _logger.LogInformation("Remove genre from cache");
        await _cache.RemoveAsync(CacheKey.Genre);
        return genre.MapToViewModel();
    }
}
