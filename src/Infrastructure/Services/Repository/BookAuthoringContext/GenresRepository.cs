using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class GenresRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Genres>(bookAuthoringDbContext), IGenresRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<Genres?> FindActiveByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var genre = await _bookAuthoringDbContext.Genres
                    .Where(GenreFilter.ById(id))
                    .FirstOrDefaultAsync(cancellationToken);
        return genre;
    }

    public async Task<Genres?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .IgnoreQueryFilters()
            .Where(GenreFilter.ById(id))
            .FirstOrDefaultAsync(cancellationToken);
        return genre;
    }
    
    public async Task<IReadOnlyCollection<Genres>> FindActiveByIdsAsync(CancellationToken cancellationToken, bool ignore = false, params Guid[] ids)
    {
        var genre = _bookAuthoringDbContext.Genres
            .AsNoTracking();
        if (!ignore)
        {
            genre = genre.IgnoreQueryFilters();
        }
        var result = await genre.Where(GenreFilter.ConstanisId(ids))
            .ToListAsync(cancellationToken);
        return result.AsReadOnly();
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

    public async Task UpdateBulkAsync(IReadOnlyCollection<Genres> genres, CancellationToken cancellationToken)
    {
        await _bookAuthoringDbContext.BulkUpdateAsync(genres, cancellationToken: cancellationToken);
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
