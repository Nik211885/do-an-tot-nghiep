using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class CopyrightChapter : BaseEntity, IAggregateRoot
{
    public Guid AuthorId { get; private set; }
    public Guid BookId { get; private set; }
    public string BookTitle { get; private set; }
    public Guid ChapterId { get; private set; }
    public string ChapterTitle { get; private set; }
    public string ChapterContent { get; private set; }
    public string ChapterContentPlainText { get; private set; }
    public DigitalSignature DigitalSignature { get; private set; }
    public DateTimeOffset DateTimeCopyright { get; private set; }
    // If book has report it will remove in system
    public bool IsActive { get; private set; }
    protected CopyrightChapter(){}
    private CopyrightChapter(Guid authorId, Guid bookId, Guid chapterId, string bookTitle,
        string chapterTitle, string chapterContent, string chapterContentPlainText, string signatureValue,
        string signatureAlgorithm)
    {
        var signature = DigitalSignature.Create(signatureValue, signatureAlgorithm);
        AuthorId = authorId;
        BookId = bookId;
        BookTitle = bookTitle;
        ChapterId = chapterId;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        ChapterContentPlainText = chapterContentPlainText;
        DigitalSignature = signature;
        DateTimeCopyright = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    public static CopyrightChapter Create(Guid authorId, Guid bookId, Guid chapterId, string bookTitle,
        string chapterTitle, string chapterContent, string chapterContentPlainText, string signatureValue,
        string signatureAlgorithm)
    {
        return new CopyrightChapter(authorId, bookId, chapterId, bookTitle, chapterTitle, chapterContent,
            chapterContentPlainText, signatureValue, signatureAlgorithm);
    }

    public void Active()
    {
        IsActive = true;
    }

    public void UnActive()
    {
        IsActive = false;
    }

    public void Update(string bookTitle,string chapterTitle, string chapterContent, string chapterContentPlainText,
            string signatureValue,string signatureAlgorithm)
    {
        var newSignature = DigitalSignature.Create(signatureValue, signatureAlgorithm);
        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        ChapterContentPlainText = chapterContentPlainText;
        DigitalSignature = newSignature;
    }
}
