using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public record BookViewModel(
    Guid AuthorId,
    Guid Id,
    string Title,
    string? AvatarUrl,
    string? Description,
    DateTimeOffset CreateDateTimeOffset,
    DateTimeOffset LastUpdateDateTime,
    bool IsComplete,
    bool Visibility,
    string Slug,
    PolicyReadBook PolicyReadBook,
    BookReleaseType BookReleaseType,
    IReadOnlyCollection<Tag>? Tags,
    IReadOnlyCollection<Guid> GenreIds
);


public static class BookViewModelMappingExtension
{
    public static BookViewModel MapToViewModel(this Book book)
    {
        return new BookViewModel(
            AuthorId: book.CreatedUerId,
            Id: book.Id,
            Title: book.Title,
            AvatarUrl: book.AvatarUrl,
            Description: book.Description,
            CreateDateTimeOffset: book.CreatedDateTime,
            LastUpdateDateTime: book.LastUpdateDateTime,
            IsComplete: book.IsComplete,
            Visibility: book.Visibility,
            Slug: book.Slug,
            PolicyReadBook: book.PolicyReadBook,
            BookReleaseType: book.BookReleaseType,
            Tags: book.Tags,
            GenreIds: book.Genres.Select(x => x.GenreId).ToList()
        );
    }

    public static IReadOnlyCollection<BookViewModel> MapToViewModel(this IReadOnlyCollection<Book> books)
    {
        return books.Select(MapToViewModel).ToList();
    }
}

