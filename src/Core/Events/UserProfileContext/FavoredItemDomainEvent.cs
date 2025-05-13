using Core.BoundContext.UserProfileContext.UserFavoriteAggregate;

namespace Core.Events.UserProfileContext;

public class FavoredItemDomainEvent(Guid userFavoredId, 
    Guid favoredItemId, FavoriteItemType type)
    : IEvent
{
    public Guid UserFavoredId { get; } = userFavoredId;
    public Guid FavoredItemId { get; } = favoredItemId;
    public FavoriteItemType Type { get; } = type;
}
