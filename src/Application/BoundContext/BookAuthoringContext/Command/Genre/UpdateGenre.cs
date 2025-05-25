using System.Security.Principal;
using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;
using ThrowHelper = Application.Exceptions.ThrowHelper;

namespace Application.BoundContext.BookAuthoringContext.Command.Genre;

public record UpdateGenreRequest(string Name, string Description, string AvatarUrl);

public record UpdateGenreCommand(Guid Id, UpdateGenreRequest Request) 
    : IBookAuthoringCommand<GenreViewModel>;

public class UpdateGenreCommandHandler(
    IGenresRepository genresRepository,
    ICache cache,
    ILogger<UpdateGenreCommandHandler> logger)
    : ICommandHandler<UpdateGenreCommand, GenreViewModel>
{
    private readonly ICache _cache = cache;
    private readonly IGenresRepository _genresRepository = genresRepository;
    private readonly ILogger<UpdateGenreCommandHandler> _logger = logger;
    public async Task<GenreViewModel> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _genresRepository.FindByIdAsync(request.Id,cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(genre, GenreValidationMessage.NoFoundGenreById(request.Id));
        var updateRequest = request.Request;
        genre.Update(
            name: updateRequest.Name,
            description:updateRequest.Description,
            avatarUrl:updateRequest.AvatarUrl,
            slug: updateRequest.Name.CreateSlug()
            );
        _genresRepository.Update(genre);
        await _genresRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        await _cache.RemoveAsync(CacheKey.Genre);
        _logger.LogInformation("Update genre success, remove genre from cache");
        return genre.MapToViewModel();
    }
}
