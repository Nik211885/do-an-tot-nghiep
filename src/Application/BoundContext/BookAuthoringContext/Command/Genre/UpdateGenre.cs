using System.Security.Principal;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
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
    ILogger<UpdateGenreCommandHandler> logger)
    : ICommandHandler<UpdateGenreCommand, GenreViewModel>
{
    private readonly IGenresRepository _genresRepository = genresRepository;
    private readonly ILogger<UpdateGenreCommandHandler> _logger = logger;
    public async Task<GenreViewModel> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _genresRepository.GetGenresByIdAsync(request.Id,cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(genre, "Thể loại", new(){["Định danh"] = request.Id.ToString()});
        var updateRequest = request.Request;
        genre.Update(
            name: updateRequest.Name,
            description:updateRequest.Description,
            avatarUrl:updateRequest.AvatarUrl,
            slug: updateRequest.Name.CreateSlug()
            );
        _genresRepository.UpdateEntity(genre);
        await _genresRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return genre.MapToViewModel();
    }
}
