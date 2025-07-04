using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Book;

public record MarkCompletedBookCommand(Guid Id) 
    : IBookAuthoringCommand<BookViewModel>;


public class MarkCompleteBookCommandHandler(ILogger<MarkCompletedBookCommand> logger, IBookRepository bookRepository)
    : ICommandHandler<MarkCompletedBookCommand, BookViewModel>
{
    private readonly ILogger<MarkCompletedBookCommand> _logger = logger;

    private readonly IBookRepository _bookRepository = bookRepository;
    public async Task<BookViewModel> Handle(MarkCompletedBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book,"Sách");
        book.MarkCompletedBook();
        _bookRepository.Update(book);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Mark book with completed is {@Completd}", book.IsComplete);
        return book.MapToViewModel();
    }
}
