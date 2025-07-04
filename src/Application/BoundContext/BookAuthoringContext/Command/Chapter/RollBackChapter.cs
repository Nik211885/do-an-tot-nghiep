using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public record RollBackChapterCommand(Guid ChapterId, Guid ChapterVersionId)
    : IBookAuthoringCommand<ChapterViewModel>;

public class RollBackChapterCommandHandler(
    ILogger<RollBackChapterCommandHandler> logger,
    ICache cache,
    IChapterRepository chapterRepository)
    : ICommandHandler<RollBackChapterCommand, ChapterViewModel>
{
    private readonly ILogger<RollBackChapterCommandHandler> _logger = logger;
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly ICache _cache = cache;
    public async Task<ChapterViewModel> Handle(RollBackChapterCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _chapterRepository.FindByIdAsync(request.ChapterId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, ChapterValidationMessage.NotFoundChapterById(request.ChapterId));
        chapter.LockedCanNotBeChanged();
        var chapterDiffDataKey = string.Format(CacheKey.ChapterRollBack, request.ChapterId, request.ChapterVersionId);
        var chapterDiffData = await _cache.GetAsync<ChapterRollBackData>(chapterDiffDataKey);
        if (chapterDiffData is null)
        {
            _logger.LogInformation("Don't find chapter version diff content has key: {cacheKey} request: {request}",
                chapterDiffDataKey, request);
            // Calculate diff data   
            chapterDiffData = chapter.ChapterVersionRollBack(request.ChapterVersionId);
            await _cache.SetAsync(chapterDiffDataKey,chapterDiffData, 300);
            
            _logger.LogInformation("Save chapter diff content in cache server key: {key} data: {data}",
                chapterDiffDataKey, chapterDiffData);
        }
        // When i compare data don't need remove cache because it identifies
        // in chapter and chapter versions if you want to rollback chapter version manytime
        // it oke but in fact rule domain don't change anything it don't create new chapter version
        chapter.UpdateChapter(
            newContent:chapterDiffData.Content,
            title: chapterDiffData.Title,
            diffTitle: chapterDiffData.Title.GetDelta(chapter.Title),
            diffContent: chapterDiffData.Content.GetDelta(chapter.Content),
            slug: chapterDiffData.Title.CreateSlug(),
            chapterNumber: chapter.ChapterNumber
            );
        _logger.LogInformation("Update chapter request: {request}", request);
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Update chapter success request: {request}", request);
        return chapter.MapToViewModel();
    }
}
