using Application.BoundContext.BookReviewContext.Queries;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Queries.BookReview;

public class CommentQueries(BookReviewDbContext bookReviewDbContext) 
    : ICommentQueries
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
}
