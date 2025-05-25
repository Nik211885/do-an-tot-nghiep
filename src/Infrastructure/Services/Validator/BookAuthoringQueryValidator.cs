using Application.BoundContext.BookAuthoringContext.Validator;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Validator;

public class BookAuthoringQueryValidator(BookAuthoringDbContext bookAuthoringDbContext)
    : IBookAuthoringQueryValidator
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<bool> GenreBeUniqueNameAsync(string name, CancellationToken cancellationToken, Guid? compareGenreId = null)
    {
        if (compareGenreId is null)
        {
            return !await _bookAuthoringDbContext.Genres
                .IgnoreQueryFilters()
                .AsNoTracking()
                .AnyAsync(g => g.Name == name, cancellationToken);
        }
    
        return await _bookAuthoringDbContext.Genres
            .IgnoreQueryFilters()
            .AsNoTracking()
            .AnyAsync(g => g.Name == name && g.Id == compareGenreId, cancellationToken);
    }

    public async Task<bool> BookIsExitedAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _bookAuthoringDbContext.Books.AnyAsync(b => b.Id == id, cancellationToken: cancellationToken);
    }
}
