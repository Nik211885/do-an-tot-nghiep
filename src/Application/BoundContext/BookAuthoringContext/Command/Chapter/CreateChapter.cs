using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.Validator;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Helper;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;
using ThrowHelper = Application.Exceptions.ThrowHelper;
using ChapterCore = Core.BoundContext.BookAuthoringContext.ChapterAggregate.Chapter;

namespace Application.BoundContext.BookAuthoringContext.Command.Chapter;

public record CreateChapterCommand(Guid BookId, string Title, string Content)
    : IBookAuthoringCommand<ChapterViewModel>;

public class CreateChapterCommandHandler(IChapterRepository chapterRepository,
    IBookAuthoringQueryValidator bookAuthoringQueryValidator,
    ILogger<CreateChapterCommandHandler> logger) 
    : ICommandHandler<CreateChapterCommand, ChapterViewModel>
{
    private readonly IBookAuthoringQueryValidator _bookAuthoringQueryValidator = bookAuthoringQueryValidator;
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly ILogger<CreateChapterCommandHandler> _logger = logger;
    public async Task<ChapterViewModel> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
    {
        var bookIsExited = await _bookAuthoringQueryValidator.BookIsExitedAsync(request.BookId, cancellationToken);
        if (!bookIsExited)
        {
            ThrowHelper.ThrowNotFound(BookValidationMessages.NoFoundBookById(request.BookId));
        }
        var chapter = ChapterCore.Create(
            bookId: request.BookId,
            title: request.Title,
            content: request.Content,
            slug: request.Title.CreateSlug()
        );
        _logger.LogInformation("Create new chapter {chapter} for book {bookId}",
            request, request.BookId);
        _chapterRepository.Create(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Create new chapter {chapter} success",chapter);
        return chapter.MapToViewModel();
    }
}
