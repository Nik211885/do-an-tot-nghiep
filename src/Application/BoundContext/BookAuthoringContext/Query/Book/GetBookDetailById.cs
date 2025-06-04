using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Book;

public record GetBookDetailByIdQuery(Guid Id) :
    IQuery<BookViewModel?>;

public class GetBookDetailByIdQueryHandler(IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetBookDetailByIdQuery, BookViewModel?>
{
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    public async Task<BookViewModel?> Handle(GetBookDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookAuthoringQueries.FindBookByIdAsync(request.Id, cancellationToken);
        return result;
    }
}
