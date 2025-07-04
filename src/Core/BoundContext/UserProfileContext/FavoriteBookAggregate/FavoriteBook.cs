using Core.Entities;
using Core.Events.UserProfileContext;
using Core.Interfaces;
using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

public class FavoriteBook : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; }
    public Guid FavoriteBookId { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    protected FavoriteBook(){}
    private FavoriteBook(Guid userFavoriteBookId,Guid favoriteBookId)
    {
        FavoriteBookId = favoriteBookId;
        UserId = userFavoriteBookId;
        CreatedOn = DateTime.UtcNow;
        RaiseDomainEvent(new FavoredBookDomainEvent(this));
    }
    public static FavoriteBook Create(Guid userFavoriteBook, Guid favoriteBookId)
    {
        return new FavoriteBook(userFavoriteBook,favoriteBookId);
    }

    public void UnFavorite()
    {
        RaiseDomainEvent(new UnFavoredBookDomainEvent(this));
    }
}
