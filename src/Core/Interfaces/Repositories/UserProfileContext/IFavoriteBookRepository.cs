using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface IFavoriteBookRepository
    : IRepository<FavoriteBook>
{
    FavoriteBook Create(FavoriteBook favoriteBook);
    void Delete(FavoriteBook favoriteBook);
}
