using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Query.Genre;

public class GetAllGenresQuery
    : IQuery<IReadOnlyCollection<GenreViewModel>>;

public class GetAllGenresQueryHandler(ICache cache,
    ILogger<GetAllGenresQueryHandler> logger,
    IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetAllGenresQuery,IReadOnlyCollection<GenreViewModel>>
{
    private readonly ILogger<GetAllGenresQueryHandler> _logger = logger;
    private readonly ICache _cache = cache;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;   
    public async Task<IReadOnlyCollection<GenreViewModel>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKey.Genre;
        var genres = await _cache.GetAsync<IReadOnlyCollection<GenreViewModel>>(cacheKey);
        if (genres != null)
        {
            return genres;
        }

        genres = await _bookAuthoringQueries.FindGenresActiveAsync(cancellationToken);
        _logger.LogInformation("Get all genres and set to cache is 6 minute");
        await _cache.SetAsync(cacheKey, genres, 600);
        return genres;
    }
}
