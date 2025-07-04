namespace Core.Events.BookReviewContext;

public class CommentCreatedDomainEvent : IEvent
{
    public Guid CommentId { get; }
    public Guid BookReviewId { get; }
    public string Content { get; }

    public CommentCreatedDomainEvent(Guid commentId, Guid bookReviewId, string content)
    {
        CommentId = commentId;
        BookReviewId = bookReviewId;
        Content = content;
    }
}
