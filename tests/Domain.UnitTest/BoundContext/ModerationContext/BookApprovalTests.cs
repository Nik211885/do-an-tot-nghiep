/*
using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Domain.UnitTest.BoundContext.ModerationContext;

public class BookApprovalTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var bookId = Guid.NewGuid();
        var chapterId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var approval = BookApproval.Create(bookId, chapterId, authorId, "Book", "Chapter", 1, "slug", "content");
        Assert.Equal(bookId, approval.BookId);
        Assert.Equal(chapterId, approval.ChapterId);
        Assert.Equal(authorId, approval.AuthorId);
        Assert.Equal("Book", approval.BookTitle);
        Assert.Equal("Chapter", approval.ChapterTitle);
        Assert.Equal(1, approval.ChapterNumber);
        Assert.Equal("slug", approval.ChapterSlug);
        Assert.Equal("content", approval.ChapterContent);
        Assert.Equal(BookApprovalStatus.Pending, approval.Status);
    }

    [Fact]
    public void Approve_Should_Set_Status_And_Add_Decision_And_Copyright()
    {
        var approval = BookApproval.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Book", "Chapter", 1, "slug", "content");
        approval.Approve(Guid.NewGuid(), "ok", false);
        Assert.Equal(BookApprovalStatus.Approved, approval.Status);
        Assert.NotNull(approval.CopyrightChapter);
        Assert.Single(approval.Decision);
    }

    [Fact]
    public void Reject_Should_Set_Status_And_Add_Decision()
    {
        var approval = BookApproval.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Book", "Chapter", 1, "slug", "content");
        approval.Reject(Guid.NewGuid(), "not ok", false);
        Assert.Equal(BookApprovalStatus.Rejected, approval.Status);
        Assert.Single(approval.Decision);
    }

    [Fact]
    public void AddSignature_Should_Add_Signature_When_Approved()
    {
        var approval = BookApproval.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Book", "Chapter", 1, "slug", "content");
        approval.Approve(Guid.NewGuid(), "ok", false);
        approval.AddSignature("alg", "sig");
        Assert.NotNull(approval.CopyrightChapter.DigitalSignature);
        Assert.Equal("sig", approval.CopyrightChapter.DigitalSignature.SignatureValue);
        Assert.Equal("alg", approval.CopyrightChapter.DigitalSignature.SignatureAlgorithm);
    }

    [Fact]
    public void AddSignature_Should_Throw_If_Not_Approved()
    {
        var approval = BookApproval.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Book", "Chapter", 1, "slug", "content");
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() => approval.AddSignature("alg", "sig"));
        Assert.Contains("Chua duyet", ex.Message);
    }

    [Fact]
    public void AddSignature_Should_Throw_If_No_Content()
    {
        var approval = BookApproval.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Book", "Chapter", 1, "slug", "content");
        approval.Approve(Guid.NewGuid(), "ok", false);
        var ex = Assert.Throws<Core.Exception.BadRequestException>(() => approval.AddSignature("alg", "sig"));
        Assert.Contains("Chua co noi dung", ex.Message);
    }

    [Fact]
    public void OpenApproval_Should_Set_Status_Pending()
    {
        var approval = BookApproval.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Book", "Chapter", 1, "slug", "content");
        approval.Approve(Guid.NewGuid(), "ok", false);
        approval.OpenApproval();
        Assert.Equal(BookApprovalStatus.Pending, approval.Status);
    }
}
*/
