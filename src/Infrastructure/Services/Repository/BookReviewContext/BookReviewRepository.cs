using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.BookReviewContext;

public class BookReviewRepository(BookReviewDbContext bookReviewDbContext) 
    : Repository<BookReview>(bookReviewDbContext), IBookReviewRepository
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
}
