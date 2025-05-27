using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public record RenameChapterVersionCommand(
    Guid ChapterId,
    Guid ChapterVersionId,
    string NameVersion) 
    : IBookAuthoringCommand<string>;

public class RenameChapterVersionCommandHandler(
    ILogger<RenameChapterVersionCommand>  logger,
    IChapterRepository chapterRepository)
    : ICommandHandler<RenameChapterVersionCommand, string>
{
    private readonly ILogger<RenameChapterVersionCommand> _logger = logger;
    private readonly IChapterRepository _chapterRepository =  chapterRepository;
    public async Task<string> Handle(RenameChapterVersionCommand request, CancellationToken cancellationToken)
    {
        var chapter = await _chapterRepository.FindByIdAsync(request.ChapterId,cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, ChapterValidationMessage.NotFoundChapterById(request.ChapterId));
        chapter.UpdateNameVersion(request.ChapterVersionId, request.NameVersion);
        _logger.LogInformation("Renamed chapter {ChapterId} {NameVersion}", request.ChapterId, request.NameVersion);
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Renamed chapter {ChapterId} {NameVersion}", request.ChapterId, request.NameVersion);
        return chapter.Id.ToString();
    }
}
