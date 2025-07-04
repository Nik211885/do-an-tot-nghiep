using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Chapter;

public record GetAllChapterByBookSlugQuery(string Slug)
    : IQuery<IReadOnlyCollection<ChapterViewModel>>;

public class GetAllChapterByBookIdQueryHandler(
    ICache cache, 
    IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetAllChapterByBookSlugQuery, IReadOnlyCollection<ChapterViewModel>>
{
    private readonly ICache _cache = cache;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    public async Task<IReadOnlyCollection<ChapterViewModel>> Handle(GetAllChapterByBookSlugQuery request, CancellationToken cancellationToken)
    {
        // var chapterByBookKey = string.Format(CacheKey.ChapterByBookSlug, request.Slug);
        // var chapterByBook = await _cache.GetAsync<IReadOnlyCollection<ChapterViewModel>>(chapterByBookKey);
        // if (chapterByBook != null)
        // {
        //     return chapterByBook;
        // }
        var chapterByBook = await _bookAuthoringQueries.FindChapterByBookSlugAsync(request.Slug, cancellationToken);
        // await _cache.SetAsync(chapterByBookKey, chapterByBook,300);
        return chapterByBook;
    }
}
