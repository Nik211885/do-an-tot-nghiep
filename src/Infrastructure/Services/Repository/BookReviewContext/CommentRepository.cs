using Core.BoundContext.BookReviewContext.CommentAggregate;
using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.BookReviewContext;

public class CommentRepository(BookReviewDbContext bookReviewDbContext)
    : Repository<Comment>(bookReviewDbContext), ICommentRepository
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
    public Comment CreateComment(Comment comment)
    {
       return _bookReviewDbContext.Add(comment).Entity;
    }

    public Comment UpdateComment(Comment comment)
    {
        return _bookReviewDbContext.Update(comment).Entity;
    }

    public async Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var comment = await _bookReviewDbContext.
            Comments.FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        return comment;
    }
}
