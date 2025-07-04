using Core.BoundContext.BookReviewContext.CommentAggregate;

namespace Domain.UnitTest.BoundContext.BookReviewContext;

public class CommentTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var reviewId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var content = "Nội dung bình luận";
        var comment = Comment.Create(reviewId, userId, content);
        Assert.Equal(reviewId, comment.BookReviewId);
        Assert.Equal(userId, comment.ReviewerId);
        Assert.Equal(content, comment.Content);
        Assert.Null(comment.ParentCommentId);
        Assert.Equal(0, comment.ReplyCount);
    }

    [Fact]
    public void Create_Should_Succeed_With_ParentCommentId()
    {
        var reviewId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        var content = "Trả lời bình luận";
        var comment = Comment.Create(reviewId, userId, content, parentId);
        Assert.Equal(parentId, comment.ParentCommentId);
    }

    [Fact]
    public void UpdateComment_Should_Change_Content()
    {
        var comment = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "old");
        comment.UpdateComment("new");
        Assert.Equal("new", comment.Content);
    }

    [Fact]
    public void IncrementReplyCount_Should_Increase_ReplyCount()
    {
        var comment = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "abc");
        comment.IncrementReplyCount();
        Assert.Equal(1, comment.ReplyCount);
    }

    [Fact]
    public void DecrementReplyCount_Should_Decrease_ReplyCount()
    {
        var comment = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), "abc");
        comment.IncrementReplyCount();
        comment.DecrementReplyCount();
        Assert.Equal(0, comment.ReplyCount);
    }
}
