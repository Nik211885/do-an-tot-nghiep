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
        var requestBody = request.Request;
        _logger.LogInformation("Create delta diff for update chapter request {request}", request);
        chapter.UpdateChapter(
            newContent: requestBody.Content,
            title: requestBody.Title,
            diffTitle: requestBody.Content.GetDelta(chapter.Content),
            diffContent: requestBody.Title.GetDelta(chapter.Title),
            slug: requestBody.Title.CreateSlug()
        );
        _logger.LogInformation("Update chapter for chapter {chapter}", chapter);
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Update chapter: {chapter} success", chapter);

        return chapter.MapToViewModel();
    }
}
