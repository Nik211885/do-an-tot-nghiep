using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.Query;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Application.BoundContext.BookAuthoringContext.Queries;

public interface IBookAuthoringQueries : IApplicationQueryServicesExtension
{
    // Genre
    Task<IReadOnlyCollection<GenreViewModel>> FindGenresActiveAsync(CancellationToken cancellationToken);
    Task<GenreViewModel?> FindGenreBySlugAsync(string slug, CancellationToken cancellationToken);
    // you can use pagination in here but in fact I think  user just have some book
    // because write new book lost has many time iit calculate equals year or
    // Books
    Task<IReadOnlyCollection<BookViewModel>> FindBookForUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<BookViewModel?> FindBookBySlugAsync(string slug, CancellationToken cancellationToken);
    
    // Chapter
    Task<IReadOnlyCollection<ChapterViewModel>> FindChapterByBookSlugAsync(string slug, CancellationToken cancellationToken);
    Task<ChapterViewModel?> FindChapterBySlugAsync(string slug, CancellationToken cancellationToken);
    Task<ChapterViewModel?> FindChapterVersionByChapterIdAsync(Guid id, CancellationToken cancellationToken);
}
    
