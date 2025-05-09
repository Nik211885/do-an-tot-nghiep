using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Exception;
using Core.Message;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class Comment : BaseEntity
{
    private Guid _bookReviewId;
    public Guid ReviewerId { get; private set; }
    public string Content  { get; private set; }
    public DateTimeOffset DatetimeCommented { get; private set; }
    
    private Guid? _commentChildId;
    private List<Comment>? _commentsChilds;

    private Comment(Guid bookReviewId, Guid reviewerId, string content)
    {
        _bookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        Content = content;
        DatetimeCommented = DateTimeOffset.UtcNow;
    }

    public static Comment Create(Guid bookReviewId, Guid reviewerId, string content)
    {
        return new Comment(bookReviewId, reviewerId, content);
    }

    public void Reply(Guid replierId, string content)
    {
        var replyComment = Comment.Create(_bookReviewId, replierId, content);
        _commentsChilds ??= [];
        _commentsChilds.Add(replyComment);
        RaiseDomainEvent(new RepliedCommentDomainEvent(Id,ReviewerId,content));
    }

    public void RemoveReply(Guid replyId)
    {
        var reply = _commentsChilds?.FirstOrDefault(c => c.Id == replyId);
        if (reply is null)
        {
            throw new BadRequestException(BookReviewContextMessage.CanNotFindCommentId);
        }
        _commentsChilds?.Remove(reply);
    }
}
