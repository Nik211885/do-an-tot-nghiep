using Core.ValueObjects;

namespace Core.BoundContext.UserProfileContext.UserFavoriteAggregate;

public class FavoriteItem : ValueObject
{
    public Guid FavoriteItemId { get; private set; }
    public FavoriteItemType FavoriteItemType { get; private set; }
    public DateTime CreatedOn { get; private set; }
    private FavoriteItem(Guid favoriteItemId, FavoriteItemType favoriteItemType)
    {
        FavoriteItemType = favoriteItemType;
        CreatedOn = DateTime.UtcNow;
        FavoriteItemId = favoriteItemId;
    }

    public static FavoriteItem Create(Guid favoriteItemId, FavoriteItemType favoriteItemType)
    {
        return new FavoriteItem(favoriteItemId, favoriteItemType);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FavoriteItemId;
        yield return FavoriteItemType;
        yield return CreatedOn;
    }
}
