using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Domain.UnitTest.BoundContext.BookAuthoringContext;

public class ChapterTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var bookId = Guid.NewGuid();
        var chapter = Chapter.Create(bookId, "content", "title", "slug", 1);
        Assert.Equal(bookId, chapter.BookId);
        Assert.Equal("content", chapter.Content);
        Assert.Equal("title", chapter.Title);
        Assert.Equal("slug", chapter.Slug);
        Assert.Equal(1, chapter.ChapterNumber);
        Assert.False(chapter.IsLocked);
    }

    [Fact]
    public void SubmitAndReview_Should_Change_Status_And_Lock()
    {
        var chapter = Chapter.Create(Guid.NewGuid(), "content", "title", "slug", 1);
        chapter.SubmitAndReview();
        Assert.True(chapter.IsLocked);
        Assert.Equal(ChapterStatus.Submitted, chapter.Status);
    }

    [Fact]
    public void LockedCanNotBeChanged_Should_Throw_When_Locked()
    {
        var chapter = Chapter.Create(Guid.NewGuid(), "content", "title", "slug", 1);
        chapter.Locked();
        Assert.Throws<Core.Exception.BadRequestException>(() => chapter.LockedCanNotBeChanged());
    }

    [Fact]
    public void Unlocked_Should_Set_IsLocked_False()
    {
        var chapter = Chapter.Create(Guid.NewGuid(), "content", "title", "slug", 1);
        chapter.Locked();
        chapter.Unlocked();
        Assert.False(chapter.IsLocked);
    }

    [Fact]
    public void ModerationRejected_Should_Set_Status_And_Unlock()
    {
        var chapter = Chapter.Create(Guid.NewGuid(), "content", "title", "slug", 1);
        chapter.Locked();
        chapter.ModerationRejected();
        Assert.Equal(ChapterStatus.Rejected, chapter.Status);
        Assert.False(chapter.IsLocked);
    }

    [Fact]
    public void ModerationApproved_Should_Set_Status_And_Unlock()
    {
        var chapter = Chapter.Create(Guid.NewGuid(), "content", "title", "slug", 1);
        chapter.Locked();
        chapter.ModerationApproved();
        Assert.Equal(ChapterStatus.Approved, chapter.Status);
        Assert.False(chapter.IsLocked);
    }
}
