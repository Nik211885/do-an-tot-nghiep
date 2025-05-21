using Core.BoundContext.UserProfileContext.UserProfileAggregate;

namespace Core.Events.UserProfileContext;

public class FavoredBookDomainEvent(Guid userFavoredId, 
    Guid favoredBookId)
    : IEvent
{
    public Guid UserFavoredId { get; } = userFavoredId;
    public Guid FavoredItemId { get; } = favoredBookId;
}
