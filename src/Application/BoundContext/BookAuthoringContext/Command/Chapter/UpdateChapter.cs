using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public record UpdateChapterRequest(string Title, string Content);

public record UpdateChapterCommand(Guid Id, UpdateChapterRequest Request)
    : IBookAuthoringCommand<ChapterViewModel>;

public class UpdateChapterCommandHandler(IChapterRepository chapterRepository,
    ILogger<UpdateChapterCommandHandler> logger)
    : ICommandHandler<UpdateChapterCommand, ChapterViewModel>
{
    
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly ILogger<UpdateChapterCommandHandler> _logger = logger;
    public async Task<ChapterViewModel> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _chapterRepository.FindById(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, "Chương",
            new (){["Định danh"] = request.Id.ToString()});
        var diffMatchPatch = new diff_match_patch();
        var requestBody = request.Request;
        _logger.LogInformation("Create delta diff for update chapter request {request}", request);
        var deltaContent = GetDelta(chapter.Content, requestBody.Content);
        var deltaTitle = GetDelta(chapter.Title, requestBody.Title);
        chapter.UpdateChapter(
            newContent: requestBody.Content,
            title: requestBody.Title,
            diffTitle: deltaTitle,
            diffContent: deltaContent,
            slug: requestBody.Title.CreateSlug()
        );
        _logger.LogInformation("Update chapter for chapter {chapter}", chapter);
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Update chapter: {chapter} success", chapter);
        string GetDelta(string oldText, string newText)
        {
            var diffValue = diffMatchPatch.diff_main(oldText, newText);
            var delta = diffMatchPatch.diff_toDelta(diffValue);
            return delta;
        }

        return chapter.MapToViewModel();
    }
}
