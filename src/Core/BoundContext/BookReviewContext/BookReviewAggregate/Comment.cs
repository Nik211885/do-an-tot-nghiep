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
    
    private Guid? _commentParentId;
    public Comment? ParentComment { get; private set; }
    
    private List<Comment>? _commentChild;
    public IReadOnlyCollection<Comment>? CommentChild => _commentChild;
    protected Comment(){}
    private Comment(Guid bookReviewId, Guid reviewerId, string content, Guid? commentParentId)
    {
        _bookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        _commentParentId = commentParentId;
        Content = content;
        DatetimeCommented = DateTimeOffset.UtcNow;
    }

    public static Comment Create(Guid bookReviewId, Guid reviewerId, string content, Guid? commentParentId)
    {
        return new Comment(bookReviewId, reviewerId, content, commentParentId);
    }

    public void Reply(Guid replierId, string content)
    {
        var replyComment = Comment.Create(_bookReviewId, replierId, content, Id);
        _commentChild ??= [];
        _commentChild.Add(replyComment);
        RaiseDomainEvent(new RepliedCommentDomainEvent(Id,ReviewerId,content));
    }

    public void RemoveReply(Guid replyId)
    {
        var reply = _commentChild?.FirstOrDefault(c => c.Id == replyId);
        if (reply is null)
        {
            throw new BadRequestException(BookReviewContextMessage.CanNotFindCommentId);
        }
        _commentChild?.Remove(reply);
    }
}
