using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Book;

public record UpdateBookRequest(
    string Title,
    string? AvatarUrl,
    string? Description,
    BookPolicy ReaderBookPolicy,
    decimal? ReaderBookPolicyPrice,
    BookReleaseType BookReleaseType,
    IReadOnlyCollection<string> TagsName,
    bool Visibility,
    IReadOnlyCollection<Guid> GenreIds);

public record UpdateBookCommand(Guid Id, UpdateBookRequest Request)
    : IBookAuthoringCommand<BookViewModel>;

public class UpdateBookCommandHandler (
        IBookRepository bookRepository,
        ILogger<UpdateBookCommandHandler> logger)
    : ICommandHandler<UpdateBookCommand, BookViewModel>
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly ILogger<UpdateBookCommandHandler> _logger = logger;
    public async Task<BookViewModel> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book, BookValidationMessages.NoFoundBookById(request.Id));
        var requestBody = request.Request;
        book.Update(
            title: requestBody.Title,
            avatarUrl: requestBody.AvatarUrl,
            description: requestBody.Description,
            slug:  requestBody.Title.CreateSlug(),
            bookPolicy: requestBody.ReaderBookPolicy,
            priceReaderBook: requestBody.ReaderBookPolicyPrice,
            visibility: requestBody.Visibility,
            releaseType: requestBody.BookReleaseType,
            tagNames: requestBody.TagsName,
            genreIds: requestBody.GenreIds
            );
        _bookRepository.Update(book);
        _logger.LogInformation("Book updated book {bookId}", book.Id);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Book updated book success {bookId}", book.Id);
        return book.MapToViewModel();
    }
}
