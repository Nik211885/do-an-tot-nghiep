using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;

namespace Application.BoundContext.BookAuthoringContext.Query;

public record GetAllGenreActiveQuery : IQuery<IReadOnlyCollection<Genres>>;

public class GetAllGenreActiveQueryHandler(IGenresRepository genresRepository, ICache cache)
    : IQueryHandler<GetAllGenreActiveQuery, IReadOnlyCollection<Genres>>
{
    private readonly IGenresRepository _genresRepository = genresRepository;
    private readonly ICache _cache = cache;
    public async Task<IReadOnlyCollection<Genres>> Handle(GetAllGenreActiveQuery request, CancellationToken cancellationToken)
    {
        var genreKey = "genreKey";
        var genresInCache = await _cache.GetAsync<IReadOnlyCollection<Genres>>(genreKey);
        if (genresInCache is not null)
        {
            return genresInCache;
        }
        var genresInDb = await _genresRepository.GetAllGenresAsync(cancellationToken);
        return genresInDb;
    }
}

