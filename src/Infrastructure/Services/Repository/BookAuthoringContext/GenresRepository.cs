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
    public async Task<IReadOnlyCollection<Genres>> GetAllGenresActiveAsync()
    {
        var genreActive = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .ToListAsync();
        return genreActive.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Genres>> GetAllGenresAsync()
    {
        var allGenres = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .IgnoreQueryFilters()
            .ToListAsync();
        return allGenres.AsReadOnly();
    }

    public async Task<Genres?> GetGenresActiveBySlugAsync(string slug)
    {
        var genreActive = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(GenreFilter.BySlug(slug))
            .FirstOrDefaultAsync();
        return genreActive;
    }

    public Task<Genres?> GetGenresBySlugAsync(string slug)
    {
        var genre = _bookAuthoringDbContext.Genres
            .IgnoreQueryFilters()
            .Where(GenreFilter.BySlug(slug))
            .FirstOrDefaultAsync();
        return genre;
    }

    public async Task<Genres?> GetGenresActiveById(Guid id)
    {
        var genreActive = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(GenreFilter.ById(id))
            .FirstOrDefaultAsync();
        return genreActive;
    }

    public async Task<Genres?> GetGenresByIdAsync(Guid id)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .Where(GenreFilter.ById(id))
            .FirstOrDefaultAsync();
        return genre;
    }

    public async Task<IReadOnlyCollection<Genres>> GetGenresActiveByIdsAsync(params Guid[] ids)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(GenreFilter.ConstanisId(ids))
            .ToListAsync();
        return genre.AsReadOnly();
    }

    private static class GenreFilter
    {
        public static Expression<Func<Genres, bool>> ById(Guid id)
            => g => g.Id == id;
        public static Expression<Func<Genres, bool>> BySlug(string slug)
            => g => g.Slug == slug;
        public static Expression<Func<Genres, bool>> ConstanisId(params Guid[] ids)
            => g=>ids.Contains(g.Id);
    }
}
