namespace Core.Events.BookReviewContext;

public class RepliedCommentDomainEvent(Guid commentId, Guid receiverId, string message) : IEvent
{
    public Guid CommentId { get; } = commentId;
    public Guid ReceiverId { get; } = receiverId;
    public string Message { get; } = message;
}
