using System.Linq.Expressions;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class GenresRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Genres>(bookAuthoringDbContext), IGenresRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<IReadOnlyCollection<Genres>> FindAllActiveAsync(CancellationToken cancellationToken)
    {
        var genreActive = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return genreActive.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Genres>> FindAllAsync(CancellationToken cancellationToken)
    {
        var allGenres = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ToListAsync(cancellationToken);
        return allGenres.AsReadOnly();
    }

    public async Task<Genres?> FindActiveBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var genreActive = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(GenreFilter.BySlug(slug))
            .FirstOrDefaultAsync(cancellationToken);
        return genreActive;
    }

    public Task<Genres?> FindBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var genre = _bookAuthoringDbContext.Genres
            .IgnoreQueryFilters()
            .Where(GenreFilter.BySlug(slug))
            .FirstOrDefaultAsync(cancellationToken);
        return genre;
    }

    public async Task<Genres?> FindActiveById(Guid id, CancellationToken cancellationToken)
    {
        var genreActive = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(GenreFilter.ById(id))
            .FirstOrDefaultAsync(cancellationToken);
        return genreActive;
    }

    public async Task<Genres?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .Where(GenreFilter.ById(id))
            .FirstOrDefaultAsync(cancellationToken);
        return genre;
    }

    public async Task<IReadOnlyCollection<Genres>> FindActiveByIdsAsync(CancellationToken cancellationToken, params Guid[] ids)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(GenreFilter.ConstanisId(ids))
            .ToListAsync(cancellationToken);
        return genre.AsReadOnly();
    }

    public async Task<Genres?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(GenreFilter.ByName(name))
            .FirstOrDefaultAsync(cancellationToken);
        return genre;
    }

    public Genres Create(Genres genre)
    {
        return _bookAuthoringDbContext.Genres.Add(genre).Entity;
    }

    public Genres Update(Genres genre)
    {
        _bookAuthoringDbContext.Genres.Update(genre);
        return genre;
    }

    private static class GenreFilter
    {
        public static Expression<Func<Genres, bool>> ById(Guid id)
            => g => g.Id == id;
        public static Expression<Func<Genres, bool>> BySlug(string slug)
            => g => g.Slug == slug;
        public static Expression<Func<Genres, bool>> ConstanisId(params Guid[] ids)
            => g=>ids.Contains(g.Id);
        public static Expression<Func<Genres, bool>> ByName(string name)
            => g => g.Name == name;
    }
}
