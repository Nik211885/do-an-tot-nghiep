using Core.BoundContext.UserProfileContext.FavoriteBookAggregate;

namespace Application.BoundContext.UserProfileContext.ViewModel;

public class FavoriteBookViewModel
{
    
    public Guid Id { get;  }
    public Guid UserId { get; }
    public Guid FavoriteBookId { get; } 
    public DateTimeOffset CreateDate { get; }

    public FavoriteBookViewModel(Guid id, Guid userId, Guid favoriteBookId, DateTimeOffset createDate)
    {
        Id = id;
        UserId = userId;
        FavoriteBookId = favoriteBookId;
        CreateDate = createDate;
    }
}

public static class MappingFavoriteBookExtensions
{
    public static FavoriteBookViewModel ToViewModel(this FavoriteBook favoriteBook)
    {
        return new FavoriteBookViewModel(
                id:  favoriteBook.Id,
                userId:  favoriteBook.UserId,
                favoriteBook.FavoriteBookId,
                createDate: favoriteBook.CreatedOn
            );
    }

    public static IEnumerable<FavoriteBookViewModel> ToViewModel(this IEnumerable<FavoriteBook> favoriteBooks)
    {
        return favoriteBooks.Select(ToViewModel);
    }
}
