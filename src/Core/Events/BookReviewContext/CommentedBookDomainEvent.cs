namespace Core.Events.BookReviewContext;

public class CommentedBookDomainEvent(Guid commentId, Guid bookId, string comment) : IEvent
{
    public Guid CommentId { get; } = commentId;
    public Guid BookId { get; } = bookId;
    public string Comment { get; } = comment;
}
