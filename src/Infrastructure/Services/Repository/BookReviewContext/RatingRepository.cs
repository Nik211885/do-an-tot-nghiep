using Core.BoundContext.BookReviewContext.RatingAggregate;
using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookReviewContext;

public class RatingRepository(BookReviewDbContext bookReviewDbContext) 
    : Repository<Rating>(bookReviewDbContext),
    IRatingRepository
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
    public Rating CreateRating(Rating rating)
    {
        return _bookReviewDbContext.Ratings.Add(rating).Entity;
    }

    public Rating UpdateRating(Rating rating)
    {
        return _bookReviewDbContext.Ratings.Update(rating).Entity;
    }

    public async Task<Rating?> GetRatingByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _bookReviewDbContext.Ratings
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        return result;
    }
}
