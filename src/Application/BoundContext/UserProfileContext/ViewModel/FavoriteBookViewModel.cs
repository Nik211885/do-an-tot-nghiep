using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Models;
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
            id: favoriteBook.Id,
            userId: favoriteBook.UserId,
            favoriteBook.FavoriteBookId,
            createDate: favoriteBook.CreatedOn
        );
    }
    

    public static IEnumerable<FavoriteBookViewModel> ToViewModel(this IEnumerable<FavoriteBook> favoriteBooks)
    {
        return favoriteBooks.Select(ToViewModel);
    }

    public static PaginationItem<FavoriteBookViewModelAggregate> ToPaginationAggregate(
        this PaginationItem<FavoriteBookViewModel> favoriteBook,
        IEnumerable<BookElasticModel> book,
        IEnumerable<BookReviewViewModel> reviewContext)
    {
        var aggregate = favoriteBook.Items
            .Select(x => new FavoriteBookViewModelAggregate(
                    book: book.FirstOrDefault(b=>b.Id == x.FavoriteBookId.ToString()),
                    review: reviewContext.FirstOrDefault(r=>r.BookId == x.FavoriteBookId),
                    x
                )).ToList();
        return new PaginationItem<FavoriteBookViewModelAggregate>(aggregate, favoriteBook.PageNumber,
            favoriteBook.PageSize, favoriteBook.TotalCount);
    }
}


public class FavoriteBookViewModelAggregate 
{
    public BookElasticModel? Book { get; }
    public BookReviewViewModel? Review { get; }
    public FavoriteBookViewModel? FavoriteBook { get; }

    public FavoriteBookViewModelAggregate(BookElasticModel? book, 
        BookReviewViewModel? review, FavoriteBookViewModel? favoriteBook)
    {
        Book = book;
        Review = review;
        FavoriteBook = favoriteBook;
    }
}
