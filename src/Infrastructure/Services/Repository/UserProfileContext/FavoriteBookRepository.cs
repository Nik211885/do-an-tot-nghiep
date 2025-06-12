using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.UserProfileContext;

public class FavoriteBookRepository(UserProfileDbContext dbContext) : Repository<FavoriteBook>(dbContext), IFavoriteBookRepository
{
    private readonly UserProfileDbContext _userProfileDbContext= dbContext;
    public async Task<FavoriteBook?> FindByBookAndUserAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var favoriteBook = await _userProfileDbContext.FavoriteBooks
            .Where(x=>x.UserId == userId
            && x.FavoriteBookId == bookId)
            .FirstOrDefaultAsync(cancellationToken);
        return favoriteBook; 
    }

    public FavoriteBook Create(FavoriteBook favoriteBook)
    {
        return _userProfileDbContext.FavoriteBooks.Add(favoriteBook).Entity;
    }

    public void Delete(FavoriteBook favoriteBook)
    {
        _userProfileDbContext.FavoriteBooks.Remove(favoriteBook);
    }
}
