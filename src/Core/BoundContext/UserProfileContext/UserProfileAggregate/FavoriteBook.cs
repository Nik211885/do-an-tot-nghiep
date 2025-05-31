using Core.Interfaces;
using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.UserProfileAggregate;

public class FavoriteBook 
{
    public Guid FavoriteBookId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    protected FavoriteBook(){}
    private FavoriteBook(Guid userFavoriteBookId,Guid favoriteBookId)
    {
        FavoriteBookId = favoriteBookId;
    }

    public static FavoriteBook Create(Guid userFavoriteBook, Guid favoriteBookId)
    {
        return new FavoriteBook(userFavoriteBook,favoriteBookId);
    }
}
