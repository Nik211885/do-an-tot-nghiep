using System.Reflection.Metadata;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Helper;
using Application.Models;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Infrastructure.Services.Queries;

public class BookAuthoringQueries(BookAuthoringDbContext bookAuthoringDbContext) 
    : IBookAuthoringQueries
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
    public async Task<GenreViewModel?> FindGenreByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var genres = _bookAuthoringDbContext
            .Genres.AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x=>x.Id == id); 
        var result = await genres.FirstOrDefaultAsync(cancellationToken);
        return result?.MapToViewModel();
    }

    public async Task<IReadOnlyCollection<GenreViewModel>> FindGenresActiveAsync(CancellationToken cancellationToken)
    {
        var genres = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .OrderByDescending(g=>g.CreatedDateTime)
            .ToListAsync(cancellationToken);
        return genres.MapToViewModel();
    }

    public async Task<PaginationItem<GenreViewModel>> FindGenreWithPagination(CancellationToken cancellationToken,
        PaginationRequest page,
        bool ignoreFilter = true)
    {
        var query = _bookAuthoringDbContext
            .Genres.AsNoTracking();
        if (ignoreFilter)
            query = query.IgnoreQueryFilters();
        var result = await query
            .OrderByDescending(x=>x.CreatedDateTime)
            .CreatePaginationAsync(page, GenreViewModelMappingExtensions.MapToViewModelExpression(),
            cancellationToken);
        return result;
    }

    public async Task<IReadOnlyCollection<Genres>> FindGenresByIdsAsync(CancellationToken cancellationToken = default, params Guid[] ids)
    {
        var genres = await _bookAuthoringDbContext
            .Genres
            .AsNoTracking()
            .Where(g=>ids.Contains(g.Id))
            .ToListAsync(cancellationToken);
        return genres;
    }

    public async Task<GenreViewModel?> FindGenreBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var genre  = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(x=>x.Slug == slug)
            .FirstOrDefaultAsync(cancellationToken);
        return genre?.MapToViewModel();
    }

    public async Task<GenreViewModel?> GetTopGenresHasManyBookAsync(int top, CancellationToken cancellationToken = default)
    {
        var query =
            _bookAuthoringDbContext.Genres
                .AsNoTracking()
                .OrderByDescending(x => x.CountBook)
                .Skip(top)
                .Take(1);
        var result = await query.FirstOrDefaultAsync(cancellationToken);
        return result?.MapToViewModel();
    }

    public async Task<IReadOnlyCollection<BookViewModel>> FindBookForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var books = await _bookAuthoringDbContext.Books
            .AsNoTracking()
            .Where(x=>x.CreatedUerId == userId && !x.IsComplete)
            .Include(c=>c.Tags)
            .Include(x=>x.Genres)
            .OrderByDescending(c=>c.CreatedDateTime)
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
        
        var allGenreIds = books
            .SelectMany(b => b.Genres.Select(bg => bg.GenreId))
            .Distinct()
            .ToList();
        
        var genres = await _bookAuthoringDbContext.Genres
            .AsNoTracking()
            .Where(g => allGenreIds.Contains(g.Id))
            .ToListAsync(cancellationToken);
        
        var booksResult = books.MapToViewModel(genres);
        
        return booksResult;
    }
    
    public async Task<BookViewModel?> FindBookBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var book = await _bookAuthoringDbContext.Books
            .AsNoTracking()
            .Where(x => x.Slug == slug)
            .Include(x=>x.Tags)
            .Include(x=>x.Genres)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
        if (book is null)
        {
            return null;
        }

        var genres = await _bookAuthoringDbContext.Genres
            .Where(g => book.Genres.Select(x => x.GenreId).Contains(g.Id))
            .ToListAsync(cancellationToken);
        return book.MapToViewModel(genres.MapToViewModel());

    }

    public async Task<BookViewModel?> FindBookByIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await _bookAuthoringDbContext.Books
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(x => x.Id == bookId)
            .Include(x=>x.Tags)
            .Include(x=>x.Genres)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);
        if (book is null)
        {
            return null;
        }

        var genres = await _bookAuthoringDbContext.Genres
            .Where(g => book.Genres.Select(x => x.GenreId).Contains(g.Id))
            .ToListAsync(cancellationToken);
        return book.MapToViewModel(genres.MapToViewModel());
    }

    public async Task<IReadOnlyCollection<ChapterViewModel>> FindChapterByBookSlugAsync(string slug, CancellationToken cancellationToken)
    {
        var book = await _bookAuthoringDbContext.Books
            .AsNoTracking()
            .Where(x=>x.Slug == slug)
            .FirstOrDefaultAsync(cancellationToken);
        if (book is null)
        {
            return [];
        }
        var chapter = await _bookAuthoringDbContext.Chapters
            .AsNoTracking()
            .Where(x => x.BookId == book.Id)
            .OrderByDescending(g=>g.ChapterNumber)
            .ToListAsync(cancellationToken);
        return chapter.MapToViewModel();
    }

    public async Task<ChapterViewModel?> FindChapterBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        var chapter = await _bookAuthoringDbContext.Chapters
            .AsNoTracking()
            .Where(c => c.Slug == slug)
            .FirstOrDefaultAsync(cancellationToken);
        return chapter?.MapToViewModel();
    }

    public async Task<ChapterViewModel?> FindChapterVersionByChapterIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var chapterVersion = await _bookAuthoringDbContext.Chapters
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ChapterViewModel(
                c.Id, c.BookId, c.Content, c.Title, c.IsLocked, c.Status, c.Slug,c.ChapterNumber,
                c.CreateDateTime, c.ChapterVersions.
                    OrderByDescending(cv=>cv.CreatedDateTime)
                    .Select(cv => new ChapterVersionViewModel(
                    cv.Id,
                    cv.NameVersion,
                    cv.CreatedDateTime,
                    null,
                    null,
                    cv.Version
                )).ToList())
            ).FirstOrDefaultAsync(cancellationToken);
        return chapterVersion;
    }

    public async Task<PaginationItem<BookViewModel>> FindBookWithPaginationForUserIdAsync(Guid userId,
        BookAuthoringQueriesRequest.FilterBookAuthoring? filter,
        PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var query = _bookAuthoringDbContext
            .Books.AsNoTracking()
            .Where(x=>x.IsComplete);
        if (filter is not null)
        {
            if (filter.BookReleaseType != null)
            {
                query = query.Where(x => x.BookReleaseType == filter.BookReleaseType);
            }

            if (filter.BookPolicy != null)
            {
                query = query.Where(x => x.PolicyReadBook.Policy == filter.BookPolicy);
            }

            if (!string.IsNullOrWhiteSpace(filter.Tag))
            {
                query = query.Where(x => x.Tags != null
                                         && x.Tags.Any(y => y.TagName.Contains(filter.Tag)));
            }

            if (!string.IsNullOrWhiteSpace(filter.Genre))
            {
                // Find all genre in system 
                var genres = await FindGenresActiveAsync(cancellationToken);
                query = query.Where(x => x.Genres
                    .Any(y =>
                        genres.Any(z => z.Id == y.GenreId
                        && z.Name == filter.Genre)));
            }

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(x => x.Title.Contains(filter.Title));
            }

        }

        query = query
            .OrderByDescending(x => x.CreatedDateTime)
            .Include(x => x.Tags)
            .Include(x => x.Genres)
            .AsSplitQuery();
        var bookPagination = await query.CreatePaginationAsync(
            page,
            cancellationToken
        );
         var genresByIds = bookPagination.Items
             .SelectMany(x=>x.Genres
                 .Select(y=>y.GenreId))
                    .Distinct();
         var genresByBook = await FindGenresByIdsAsync(cancellationToken, genresByIds.ToArray());
         var bookViewModel = bookPagination.Items.MapToViewModel(genresByBook);
         var bookViewModelPagination = bookViewModel.CreatePagination(page, bookPagination.TotalCount);
         return bookViewModelPagination;
    }
}

