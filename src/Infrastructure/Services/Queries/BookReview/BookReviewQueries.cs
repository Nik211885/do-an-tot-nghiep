using Application.BoundContext.BookReviewContext.Queries;
using Application.BoundContext.BookReviewContext.ViewModel;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries.BookReview;

public class BookReviewQueries(BookReviewDbContext bookReviewDbContext) 
    : IBookReviewQueries
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
    public async Task<BookReviewViewModel?> GetBookReviewByBookId(Guid bookId, CancellationToken cancellationToken = default)
    {
        var result = await _bookReviewDbContext.BookReviews
            .AsNoTracking()
            .Where(bookReview => bookReview.BookId == bookId)
            .FirstOrDefaultAsync(cancellationToken);
        return result?.MapToViewModel();
    }
}
