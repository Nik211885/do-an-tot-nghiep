using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Book;

public record GetBookDetailBySlugQuery(string Slug) 
    : IQuery<BookViewModel>;

public class GetBookDetailBySlugQueryHandler(IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetBookDetailBySlugQuery, BookViewModel>
{
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;    
    public async Task<BookViewModel> Handle(GetBookDetailBySlugQuery request, CancellationToken cancellationToken)
    {
        var bookBySlug = await _bookAuthoringQueries.FindBookBySlugAsync(request.Slug,cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookBySlug, "Sách");
        return bookBySlug;
    }
}
