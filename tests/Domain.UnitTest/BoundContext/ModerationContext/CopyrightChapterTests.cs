/*
using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Domain.UnitTest.BoundContext.ModerationContext;

public class CopyrightChapterTests
{
    [Fact]
    public void Create_Should_Succeed_Without_Signature()
    {
        var chapter = CopyrightChapter.Create("Book", "Chapter", "Content", "slug", 1);
        Assert.Equal("Book", chapter.BookTitle);
        Assert.Equal("Chapter", chapter.ChapterTitle);
        Assert.Equal("Content", chapter.ChapterContent);
        Assert.Equal("slug", chapter.ChapterSlug);
        Assert.Equal(1, chapter.ChapterNumber);
        Assert.Null(chapter.DigitalSignature);
    }

    [Fact]
    public void Create_Should_Succeed_With_Signature()
    {
        var chapter = CopyrightChapter.Create("Book", "Chapter", "Content", "sig", "alg", "slug", 1);
        Assert.NotNull(chapter.DigitalSignature);
        Assert.Equal("sig", chapter.DigitalSignature.SignatureValue);
        Assert.Equal("alg", chapter.DigitalSignature.SignatureAlgorithm);
    }

    [Fact]
    public void AddSignature_Should_Add_DigitalSignature()
    {
        var chapter = CopyrightChapter.Create("Book", "Chapter", "Content", "slug", 1);
        chapter.AddSignature("alg", "sig");
        Assert.NotNull(chapter.DigitalSignature);
        Assert.Equal("sig", chapter.DigitalSignature.SignatureValue);
        Assert.Equal("alg", chapter.DigitalSignature.SignatureAlgorithm);
    }
}
*/
