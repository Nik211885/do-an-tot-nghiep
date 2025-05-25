using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Book;

public record ChangeBookReleaseTypeCommand(Guid Id, BookReleaseType BookReleaseType)
    : IBookAuthoringCommand<BookViewModel>;

public class ChangeBookReleaseTypeCommandHandler(
    ILogger<ChangeBookReleaseTypeCommandHandler> logger,
    IBookRepository bookRepository) 
    : ICommandHandler<ChangeBookReleaseTypeCommand, BookViewModel>
{
    private readonly ILogger<ChangeBookReleaseTypeCommandHandler> _logger = logger;
    private readonly IBookRepository _bookRepository = bookRepository;
    public  async Task<BookViewModel> Handle(ChangeBookReleaseTypeCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book, BookValidationMessages.NoFoundBookById(request.Id));
        // You can't compare if bettewn in two book release type if it nature change annything it just move to databse
        book.UpdateBookReleaseType(request.BookReleaseType);
        _logger.LogInformation("Book update release type: {@BookId} {@RealeaseType}", book.Id, request.BookReleaseType);
        _bookRepository.Update(book);
        _logger.LogInformation("Book update release type has success {@BookId} {@RealeaseType}", book.Id, request.BookReleaseType);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return book.MapToViewModel();
    }
}
