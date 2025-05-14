using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;

namespace Application.BoundContext.BookAuthoringContext.Command;

public record CreateBookCommand() : ICommand<Book>;

public class CreateBookCommandHandler(IBookRepository bookRepository, IGenresRepository genresRepository) 
    : ICommandHandler<CreateBookCommand, Book>
{
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IGenresRepository _genresRepository = genresRepository;
    public Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
