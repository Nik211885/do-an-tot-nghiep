using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public record SubmitAndReviewChapterCommand(Guid Id)
    : IBookAuthoringCommand<ChapterViewModel>;

public class SubmitAndReviewChapterCommandHandler(
    ILogger<SubmitAndReviewChapterCommandHandler> logger,
    IIdentityProvider identityProvider,
    IChapterRepository chapterRepository)
    : ICommandHandler<SubmitAndReviewChapterCommand, ChapterViewModel>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly ILogger<SubmitAndReviewChapterCommandHandler> _logger = logger;
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    public async Task<ChapterViewModel> Handle(SubmitAndReviewChapterCommand request, CancellationToken cancellationToken)
    {
        //  in here you should moderation context know user has submmit and review you will send back integration event 
        // in to message bus
        var chapter = await _chapterRepository.FindByIdAsync(request.Id,cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapter, ChapterValidationMessage.NotFoundChapterById(request.Id));
        chapter.SubmitAndReview();
        _logger.LogInformation("Submit and review chapterId: {chapterId}", chapter.Id);
        _chapterRepository.Update(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Submit and review chapter is successfully submitted {chapterId}", chapter.Id);
        return chapter.MapToViewModel();
    }
}
