using Core.Entities;
using Core.Events.UserProfileContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.UserProfileContext.UserFavoriteAggregate;

public class UserFavorite 
    : BaseEntity, IAggregateRoot
{
    public Guid UserId { get; private set; } 
    private List<FavoriteItem> _favoriteItems;
    public IReadOnlyCollection<FavoriteItem> FavoriteItems => _favoriteItems.AsReadOnly();

    private UserFavorite(Guid userId)
    {
        UserId = userId;
    }

    public static UserFavorite Create(Guid userId)
    {
        return new UserFavorite(userId);
    }

    public void FavoriteItem(Guid favoriteItemId, FavoriteItemType favoriteItemType)
    {
        var favoredItemExits = _favoriteItems.FirstOrDefault(x => x.FavoriteItemId == favoriteItemId && x.FavoriteItemType == favoriteItemType);
        if (favoredItemExits is not null)
        {
            throw new BadRequestException(UserProfileContextMessage.YouHasFavoriteItem);
        }
        _favoriteItems.Add(UserFavoriteAggregate.FavoriteItem.Create(favoriteItemId, favoriteItemType));
        RaiseDomainEvent(new FavoredItemDomainEvent(UserId, favoriteItemId, favoriteItemType));
    }

    public void RemoveFavoriteItem(Guid favoriteItemId, FavoriteItemType favoriteItemType)
    {
        var favoredItemExits = _favoriteItems.FirstOrDefault(x => x.FavoriteItemId == favoriteItemId && x.FavoriteItemType == favoriteItemType);
        if (favoredItemExits is null)
        {
            throw new BadRequestException(UserProfileContextMessage.YouDontHaveFavoriteItem);
        }
        _favoriteItems.Remove(favoredItemExits);
    }
}
