using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Interfaces.Query;

namespace Application.BoundContext.BookReviewContext.Queries;

public interface IBookReviewQueries : IApplicationQueryServicesExtension
{
    Task<BookReviewViewModel?>  GetBookReviewByBookId(Guid bookId, CancellationToken cancellationToken = default);
}
