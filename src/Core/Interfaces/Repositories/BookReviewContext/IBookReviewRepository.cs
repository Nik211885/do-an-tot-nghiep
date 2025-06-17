using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;

namespace Core.Interfaces.Repositories.BookReviewContext;

public interface IBookReviewRepository
    : IRepository<BookReview>
{
    BookReview CreateBookReview(BookReview bookReview);
    BookReview UpdateBookReview(BookReview bookReview);
    bool Delete(BookReview bookReview);
    Task<BookReview?> GetBookReviewByBookIdAsync(Guid bookId, CancellationToken cancellationToken);
    Task<BookReview?> GetBookReviewByIdAsync(Guid id, CancellationToken cancellationToken);
}
