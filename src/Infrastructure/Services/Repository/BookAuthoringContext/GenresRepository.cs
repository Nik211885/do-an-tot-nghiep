using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class GenresRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Genres>(bookAuthoringDbContext), IGenresRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<Genres?> FindByIdAsync(Guid id, bool isActive = true)
    {
        var genre = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == id && x.IsActive == isActive);
        return genre;
    }

    public async Task<IReadOnlyCollection<Genres>> GetAllGenresAsync()
    {
        var genres = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .ToListAsync();
        return genres.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Genres>> GetAllGenresAsync(bool isActive)
    {
        var genres = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(x => x.IsActive == isActive)
            .ToListAsync();
        return genres.AsReadOnly();
    }
    
}
