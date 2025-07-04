

using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

namespace Core.Events.UserProfileContext;

public class UnFavoredBookDomainEvent(FavoriteBook favoriteBook)
    : IEvent
{
    public FavoriteBook UnFavoredBook { get; } = favoriteBook;
}
