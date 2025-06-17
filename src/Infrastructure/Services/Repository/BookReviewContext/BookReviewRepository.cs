using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.Interfaces;
using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookReviewContext;

public class BookReviewRepository(BookReviewDbContext bookReviewDbContext) 
    : Repository<BookReview>(bookReviewDbContext), IBookReviewRepository
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
    public BookReview CreateBookReview(BookReview bookReview)
    {
        return _bookReviewDbContext.Add(bookReview).Entity;
    }

    public BookReview UpdateBookReview(BookReview bookReview)
    {
        return _bookReviewDbContext.Update(bookReview).Entity;
    }

    public bool Delete(BookReview bookReview)
    {
        _bookReviewDbContext.BookReviews.Remove(bookReview);
        return true;
    }

    public async Task<BookReview?> GetBookReviewByBookIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        var bookReview = await _bookReviewDbContext.
            BookReviews.FirstOrDefaultAsync(x=>x.BookId ==  bookId, cancellationToken);
        return bookReview;
    }

    public async Task<BookReview?> GetBookReviewByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var bookReview = await _bookReviewDbContext.BookReviews.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        return bookReview;
    }
}
