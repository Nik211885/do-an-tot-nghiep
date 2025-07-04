using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.BoundContext.BookAuthoringContext.Queries;

public interface IBookAuthoringQueries : IApplicationQueryServicesExtension
{
    // Genre
    Task<GenreViewModel?> FindGenreByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<GenreViewModel>> FindGenresActiveAsync(CancellationToken cancellationToken);
    Task<PaginationItem<GenreViewModel>> FindGenreWithPagination(CancellationToken cancellationToken,PaginationRequest page, bool ignoreFilter = true);
    Task<IReadOnlyCollection<Genres>> FindGenresByIdsAsync(CancellationToken cancellationToken = default, params Guid[] ids);
    Task<GenreViewModel?> FindGenreBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<GenreViewModel?> GetTopGenresHasManyBookAsync(int top, CancellationToken cancellationToken = default);
    // you can use pagination in here but in fact I think  user just have some book
    // because write new book lost has many time iit calculate equals year or
    // Books
    Task<IReadOnlyCollection<BookViewModel>> FindBookForUserAsync(Guid userId, CancellationToken cancellationToken);
    
    Task<BookViewModel?> FindBookBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<BookViewModel?> FindBookByIdAsync(Guid bookId, CancellationToken cancellationToken);

    Task<PaginationItem<BookViewModel>> FindBookWithPaginationForUserIdAsync(Guid userId,
        BookAuthoringQueriesRequest.FilterBookAuthoring? filter,
        PaginationRequest page,
        CancellationToken cancellationToken = default);
    
    // Chapter
    Task<IReadOnlyCollection<ChapterViewModel>> FindChapterByBookSlugAsync(string slug, CancellationToken cancellationToken);
    Task<ChapterViewModel?> FindChapterBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<ChapterViewModel?> FindChapterVersionByChapterIdAsync(Guid id, CancellationToken cancellationToken);
}
    
public class BookAuthoringQueriesRequest
{
    public record FilterBookAuthoring(
        BookReleaseType? BookReleaseType,
        BookPolicy? BookPolicy,
        string? Tag,
        string? Genre,
        string? Title
    );
}
