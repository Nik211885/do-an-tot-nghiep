
using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

namespace Core.Events.UserProfileContext;

public class FavoredBookDomainEvent(FavoriteBook userFavoredId)
    : IEvent
{
    public FavoriteBook FavoriteBook { get; } = userFavoredId;
}
