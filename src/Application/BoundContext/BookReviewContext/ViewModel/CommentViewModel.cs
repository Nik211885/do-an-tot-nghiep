using Core.BoundContext.BookReviewContext.CommentAggregate;

namespace Application.BoundContext.BookReviewContext.ViewModel;

public class CommentViewModel
{
    public Guid Id { get; }
    public Guid BookReviewId { get;  }
    public Guid ReviewerId { get;  }
    public string Content { get;  }
    public DateTimeOffset DateCreated { get; } 
    public Guid? ParentCommentId { get; }
    public int ReplyCount { get; }

    public CommentViewModel(Guid id, Guid bookReviewId, Guid reviewerId, string content, DateTimeOffset dateCreated, Guid? parentCommentId, int replyCount)
    {
        Id = id;
        BookReviewId = bookReviewId;
        ReviewerId = reviewerId;
        Content = content;
        DateCreated = dateCreated;
        ParentCommentId = parentCommentId;
        ReplyCount = replyCount;
    }
}

public static class CommentMappingExtension
{
    public static CommentViewModel MapToViewModel(this Comment comment)
    {
        return new CommentViewModel(
            id: comment.Id,
            bookReviewId: comment.BookReviewId,
            reviewerId: comment.ReviewerId,
            content: comment.Content,
            dateCreated: comment.DatetimeCommented,
            parentCommentId: comment.ParentCommentId,
            replyCount: comment.ReplyCount
        );
    }

    public static IReadOnlyCollection<CommentViewModel> MapToViewModel(this IEnumerable<Comment> comments)
    {
        return comments.Select(MapToViewModel).ToList();
    }
}
