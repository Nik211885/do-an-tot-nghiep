﻿using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Genre;

public record ChangeActiveGenreCommand(Guid Id) 
    : IBookAuthoringCommand<GenreViewModel>;

public class ChangeActiveGenreCommandHandler(IGenresRepository genresRepository,
    ILogger<ChangeActiveGenreCommandHandler> logger,
    IIdentityProvider identityProvider) 
    : ICommandHandler<ChangeActiveGenreCommand, GenreViewModel>
{
    private readonly IGenresRepository _genresRepository = genresRepository;
    private readonly ILogger<ChangeActiveGenreCommandHandler> _logger = logger;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<GenreViewModel> Handle(ChangeActiveGenreCommand request, CancellationToken cancellationToken)
    {
        var genre = await _genresRepository
            .FindByIdAsync(id:request.Id, cancellationToken: cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(genre, "Thể loại", new (){["Định danh"] = request.Id.ToString()});
        _logger.LogInformation("User {userId} has change active for genre {genreName}", 
            _identityProvider.UserIdentity(), genre.Name);
        genre.ChangeActive();
        _genresRepository.Update(genre);
        await _genresRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("User {userId} has change genre {genreName} with information changed {changed}", 
            _identityProvider.UserIdentity(), genre.Name, genre);
        return genre.MapToViewModel();
    }
}
