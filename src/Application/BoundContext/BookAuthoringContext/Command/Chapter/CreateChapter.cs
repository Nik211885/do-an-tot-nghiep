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
    IBookRepository bookRepository,
    ILogger<CreateChapterCommandHandler> logger,
    IIdentityProvider identityProvider) 
    : ICommandHandler<CreateChapterCommand, ChapterViewModel>
{
    private readonly IChapterRepository _chapterRepository = chapterRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly ILogger<CreateChapterCommandHandler> _logger = logger;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<ChapterViewModel> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(request.BookId, cancellationToken);  
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book, "Sách",
            new(){["Định danh"] = request.BookId.ToString()});
        var chapter = ChapterCore.Create(
            bookId: request.BookId,
            title: request.Title,
            content: request.Content,
            slug: request.Title.CreateSlug()
        );
        _logger.LogInformation("Create new chapter {chapter} for book {bookId}",
            request, book.Id);
        _chapterRepository.Create(chapter);
        await _chapterRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Create new chapter {chapter} success",chapter);
        return chapter.MapToViewModel();
    }
}
