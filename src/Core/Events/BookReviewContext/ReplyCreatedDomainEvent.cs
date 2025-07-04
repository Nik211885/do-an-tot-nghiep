namespace Core.Events.BookReviewContext;

public class ReplyCreatedDomainEvent : IEvent
{
    public Guid CommentId { get; }
    public Guid BookReviewId { get; }
    public Guid ParentCommentId { get; }
    public Guid ReviewerId { get; }
    public string Content { get;}

    public ReplyCreatedDomainEvent(Guid commentId, Guid parentCommentId, Guid reviewerId, string content, Guid bookReviewId)
    {
        CommentId = commentId;
        BookReviewId = bookReviewId;
        ParentCommentId = parentCommentId;
        ReviewerId = reviewerId;
        Content = content;
    }
}
