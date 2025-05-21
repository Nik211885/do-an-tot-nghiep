using Core.Interfaces;
using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.UserProfileAggregate;

public class FavoriteBook :ValueObject
{
    public Guid UserFavoriteBookId { get; private set; }
    public Guid FavoriteBookId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    protected FavoriteBook(){}
    private FavoriteBook(Guid userFavoriteBookId,Guid favoriteBookId)
    {
        UserFavoriteBookId = userFavoriteBookId;
        FavoriteBookId = favoriteBookId;
    }

    public static FavoriteBook Create(Guid userFavoriteBook, Guid favoriteBookId)
    {
        return new FavoriteBook(userFavoriteBook,favoriteBookId);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserFavoriteBookId;
        yield return FavoriteBookId;
        yield return CreatedOn;
    }
}
