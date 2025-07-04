using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Book;

public record ChangePolicyBookCommand(Guid Id, BookPolicy BookPolicy, decimal? BookPrice)
    : IBookAuthoringCommand<BookViewModel>;


public class ChangePolicyBookCommandHandler(ILogger<ChangePolicyBookCommandHandler> logger,
    IBookRepository bookRepository)
    : ICommandHandler<ChangePolicyBookCommand, BookViewModel>
{
    private readonly ILogger<ChangePolicyBookCommandHandler> _logger = logger;
    private readonly IBookRepository _bookRepository = bookRepository;
    public async Task<BookViewModel> Handle(ChangePolicyBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book, BookValidationMessages.NoFoundBookById(request.Id));
        book.UpdatePolicyReadBook(request.BookPolicy, request.BookPrice);
        _logger.LogInformation("Updated policy reader book with id {@Bookid} {@PolicyReaderBook}",
            request.Id, request.BookPolicy);
        _bookRepository.Update(book);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Update policy reader book has success {@BookId} {@PolicyReaderBook}",
            book.Id, request.BookPolicy);
        return book.MapToViewModel();
    }
}
    
