using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface IFavoriteBookRepository
    : IRepository<FavoriteBook>
{
    Task<FavoriteBook?> FindByBookAndUserAsync(Guid userId, Guid bookId, CancellationToken cancellationToken= default);
    FavoriteBook Create(FavoriteBook favoriteBook);
    void Delete(FavoriteBook favoriteBook);
}
