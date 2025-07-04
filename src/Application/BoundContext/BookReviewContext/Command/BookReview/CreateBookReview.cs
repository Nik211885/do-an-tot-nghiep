using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.Validator;
using Core.Interfaces.Repositories.BookReviewContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookReviewContext.Command.BookReview;

public record CreateBookReviewCommand(Guid BookId) 
    : IBookReviewCommand<BookReviewViewModel>;


public class CreateBookReviewCommandHandler(
    IBookReviewRepository bookReviewRepository,
    ILogger<CreateBookReviewCommandHandler> logger,
    IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview> bookReviewValidationService)
    : ICommandHandler<CreateBookReviewCommand, BookReviewViewModel>
{
    private readonly IBookReviewRepository _bookReviewRepository = bookReviewRepository;
    private readonly ILogger<CreateBookReviewCommandHandler> _logger = logger;
    private readonly IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview> _bookReviewValidationService = bookReviewValidationService;
    public async Task<BookReviewViewModel> Handle(CreateBookReviewCommand request, CancellationToken cancellationToken)
    {
        var bookReviewExits = await _bookReviewValidationService.AnyAsync(b => b.BookId == request.BookId, cancellationToken);
        if (bookReviewExits is not null)
        {
            _logger.LogInformation("Book Review has created a new book review");
            return bookReviewExits.MapToViewModel();
        }
        var bookReview = Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview.Create(request.BookId);
        _bookReviewRepository.CreateBookReview(bookReview);
        _logger.LogInformation("Created book reviews for book {@Id}", request.BookId);
        await _bookReviewRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return bookReview.MapToViewModel();
    }
}
