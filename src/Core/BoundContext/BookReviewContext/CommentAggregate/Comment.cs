using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Interfaces;

namespace Core.BoundContext.BookReviewContext.CommentAggregate;

public class Comment : BaseEntity, IAggregateRoot
{
    public Guid BookReviewId { get; private set; }
    public Guid ReviewerId { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset DatetimeCommented { get; private set; }
    public Guid? ParentCommentId { get; private set; }
    public int ReplyCount { get; private set; }
    
    protected Comment() { }
    
    private Comment(Guid bookReviewId, Guid reviewerId, string content, Guid? parentCommentId = null)
    {
        BookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        Content = content;
        ParentCommentId = parentCommentId;
        DatetimeCommented = DateTimeOffset.UtcNow;
        ReplyCount = 0;
    }

    public static Comment Create(Guid bookReviewId, Guid reviewerId, string content, Guid? parentCommentId = null)
    {
        var comment = new Comment(bookReviewId, reviewerId, content, parentCommentId);
        
        if (parentCommentId == null)
        {
            comment.RaiseDomainEvent(new CommentCreatedDomainEvent(comment.Id, bookReviewId, content));
        }
        else
        {
            comment.RaiseDomainEvent(new ReplyCreatedDomainEvent(comment.Id, (Guid)parentCommentId, reviewerId, content, bookReviewId));
        }
        
        return comment;
    }

    public void UpdateComment(string content)
    {
        Content = content;
    }

    public void IncrementReplyCount()
    {
        ReplyCount += 1;
    }

    public void DecrementReplyCount()
    {
        if (ReplyCount > 0)
            ReplyCount -= 1;
    }
}
