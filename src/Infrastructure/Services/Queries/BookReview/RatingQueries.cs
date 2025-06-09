using Application.BoundContext.BookReviewContext.Queries;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Queries.BookReview;

public class RatingQueries(BookReviewDbContext bookReviewDbContext)
    : IRatingQueries
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
}
 
