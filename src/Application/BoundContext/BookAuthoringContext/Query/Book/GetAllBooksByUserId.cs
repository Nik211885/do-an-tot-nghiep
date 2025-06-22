using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Query.Book;

public record GetAllBooksByUserIdQuery(Guid UserId)
    : IQuery<IReadOnlyCollection<BookViewModel>>;

public class GetAllBookByUserIdQueryHandler(ICache cache,
    IBookAuthoringQueries bookAuthoringQueries)
    : IQueryHandler<GetAllBooksByUserIdQuery, IReadOnlyCollection<BookViewModel>>
{
    private readonly ICache _cache = cache;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    public async Task<IReadOnlyCollection<BookViewModel>> Handle(GetAllBooksByUserIdQuery request, CancellationToken cancellationToken)
    {
        /*var cacheKey = string.Format(CacheKey.BookByUserId, request.UserId);
        var book = await _cache.GetAsync<IReadOnlyCollection<BookViewModel>>(cacheKey);
        if (book != null)
        {
            return book;
        }*/
        var book = await _bookAuthoringQueries.FindBookForUserAsync(request.UserId, cancellationToken);
        /*await _cache.SetAsync(cacheKey, book, 600);*/
        return book;
    }
}
