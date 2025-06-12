using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.UserProfileContext;

public class FavoriteBookRepository(UserProfileDbContext dbContext) : Repository<FavoriteBook>(dbContext), IFavoriteBookRepository
{
    private readonly UserProfileDbContext _userProfileDbContext= dbContext;
    public FavoriteBook Create(FavoriteBook favoriteBook)
    {
        return _userProfileDbContext.FavoriteBooks.Add(favoriteBook).Entity;
    }

    public void Delete(FavoriteBook favoriteBook)
    {
        _userProfileDbContext.FavoriteBooks.Remove(favoriteBook);
    }
}
