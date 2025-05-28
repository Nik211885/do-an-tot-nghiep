using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command.Book;

public record DeleteBookCommand(Guid Id)
    : IBookAuthoringCommand<string>;

public class DeleteBookCommandHandler(ILogger<DeleteBookCommandHandler> logger,
    IBookRepository bookRepository) :
    ICommandHandler<DeleteBookCommand, string>
{
    private readonly ILogger<DeleteBookCommandHandler> _logger = logger;
    private readonly IBookRepository _bookRepository = bookRepository;
    public async Task<string> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book,"Sách");
        book.Delete();
        _logger.LogInformation("Delete book has id {@Id}", book.Id);
        _bookRepository.Delete(book);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Delete book has id succesful {@Id}", book.Id);
        return book.Id.ToString();
    }
}
