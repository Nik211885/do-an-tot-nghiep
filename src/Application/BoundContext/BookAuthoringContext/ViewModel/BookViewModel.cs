using System.Text.Json.Serialization;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public record PolicyReaderViewModel(decimal? Price, BookPolicy Policy);

public record TagViewModel(string TagName);

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
    PolicyReaderViewModel PolicyReadBook,
    BookReleaseType BookReleaseType,
    IReadOnlyCollection<TagViewModel>? Tags, 
    IReadOnlyCollection<GenreViewModel>? Genres
);


public static class BookViewModelMappingExtension
{
    public static BookViewModel MapToViewModel(this Book book, IReadOnlyCollection<GenreViewModel> genres)
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
            PolicyReadBook: new PolicyReaderViewModel(book.PolicyReadBook.Price, book.PolicyReadBook.Policy),
            BookReleaseType: book.BookReleaseType,
            Tags: book.Tags?.Select(x=>new TagViewModel(x.TagName)).ToList(),
            Genres: genres
        );
    }
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
            PolicyReadBook: new PolicyReaderViewModel(book.PolicyReadBook.Price, book.PolicyReadBook.Policy),
            BookReleaseType: book.BookReleaseType,
            Tags: book.Tags?.Select(x=>new TagViewModel(x.TagName)).ToList(),null
            
        );
    }

    public static IReadOnlyCollection<BookViewModel> MapToViewModel(
        this IReadOnlyCollection<Book> books,
        IReadOnlyCollection<Genres> allGenres)
    {
        var genreDict = allGenres.ToDictionary(g => g.Id);

        return books.Select(book =>
        {
            var bookGenres = book.Genres
                .Select(bg => genreDict[bg.GenreId])
                .ToList();

            var genreViewModels = bookGenres.MapToViewModel();

            return book.MapToViewModel(genreViewModels);
        }).ToList();
    }
}

