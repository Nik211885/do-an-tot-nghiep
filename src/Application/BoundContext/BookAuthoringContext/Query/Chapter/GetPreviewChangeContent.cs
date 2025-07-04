using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Query.Chapter;

public record GetPreviewChangeContentQuery(Guid ChapterId, Guid ChapterVersionId)
    : IQuery<ChapterDiffContentViewModel>;

public class GetPreviewChangeContentQueryHandler(
    ILogger<GetPreviewChangeContentQueryHandler> logger,
    IChapterRepository chapterRepository,
    ICache cache)
    : IQueryHandler<GetPreviewChangeContentQuery, ChapterDiffContentViewModel>
{
    private readonly ILogger<GetPreviewChangeContentQueryHandler> _logger = logger;
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly ICache _cache = cache;
    public async Task<ChapterDiffContentViewModel> Handle(GetPreviewChangeContentQuery request, CancellationToken cancellationToken)
    {
        // Find chapter need get pretty data roll back
        var chapter = await _chapterRepository.FindByIdAsync(request.ChapterId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, "Chương",
            new (){["Định danh"] = request.ChapterId.ToString()});
        // Check in server has cache diff create key for cache
        var chapterRollBackCacheKey = string.Format(CacheKey.ChapterRollBack, request.ChapterId.ToString(), request.ChapterVersionId.ToString());
        var chapterDataRollBack = await _cache.GetAsync<ChapterRollBackData>(chapterRollBackCacheKey);
        // Chapter version you want  to roll back
        var chapterVersionRollBack = chapter.ChapterVersions.First(x => x.Id == request.ChapterVersionId);
        if (chapterDataRollBack is null)
        {
            _logger.LogInformation("Don't find chapter version diff content has key: {cacheKey} request: {request}",
                chapterRollBackCacheKey, request);
            //roll back title and content
            // save in cache
            chapterDataRollBack = chapter.ChapterVersionRollBack(chapterVersionId: request.ChapterVersionId);
            await _cache.SetAsync(chapterRollBackCacheKey,chapterDataRollBack, 300);
            _logger.LogInformation("Save chapter diff content in cache server key: {key} data: {data}",
                chapterRollBackCacheKey, chapterDataRollBack);
        }
        _logger.LogInformation("Find chapter diff content in cache server key: {key} data: {data}",
            chapterRollBackCacheKey, chapterDataRollBack);
        // Find last version need compare
        var chapterVersionCompare = chapter.ChapterVersions
            .Zip(chapter.ChapterVersions.Skip(1), (prev, curr) => new { prev, curr })
            .FirstOrDefault(p => p.curr.Id == chapterVersionRollBack.Id)?.prev;
        // First restore to version need watch preview
        var response = new ChapterDiffContentViewModel(
            ChapterId: chapter.Id,
            ChapterVersionId: chapterVersionRollBack.Id,
            ChapterVersionName: chapterVersionRollBack.NameVersion,
            ContentPretty: chapterDataRollBack.Content.PrettyContent(chapterVersionCompare?.DiffContent),
            TitlePretty: chapterDataRollBack.Title.PrettyContent(chapterVersionCompare?.DiffTitle),
            LastModified: chapterVersionRollBack.CreatedDateTime,
            Version: chapterVersionRollBack.Version
        );
        return response;
    }
}
