using Core.BoundContext.BookReviewContext.CommentAggregate;

namespace Core.Interfaces.Repositories.BookReviewContext;

public interface ICommentRepository
    : IRepository<Comment>
{
    Comment CreateComment(Comment comment);
    Comment UpdateComment(Comment comment);
    Task<Comment?> GetCommentByIdAsync(Guid id, CancellationToken cancellationToken);
}
