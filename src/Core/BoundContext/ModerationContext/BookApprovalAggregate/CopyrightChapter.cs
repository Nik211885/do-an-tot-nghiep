using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class CopyrightChapter
{
    public string BookTitle { get; private set; }
    public string ChapterTitle { get; private set; }
    public string ChapterContent { get; private set; }
    public DigitalSignature? DigitalSignature { get; private set; }
    public DateTimeOffset DateTimeCopyright { get; private set; }
    // If book has report it will remove in system
    public bool IsActive { get; private set; }
    protected CopyrightChapter(){}
    private CopyrightChapter(string bookTitle,
        string chapterTitle, string chapterContent,  string signatureValue,
        string signatureAlgorithm)
    {
        var signature = DigitalSignature.Create(signatureValue, signatureAlgorithm);
        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        DigitalSignature = signature;
        DateTimeCopyright = DateTimeOffset.UtcNow;
        IsActive = true;
    }

    public CopyrightChapter(string bookTitle,
        string chapterTitle, string chapterContent)
    {
        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        IsActive = false;
    }
        
    public static CopyrightChapter Create(string bookTitle,
        string chapterTitle, string chapterContent,  string signatureValue,
        string signatureAlgorithm)
    {
        return new CopyrightChapter( bookTitle, chapterTitle, chapterContent
            , signatureValue, signatureAlgorithm);
    }
    public static CopyrightChapter Create(string bookTitle,
        string chapterTitle, string chapterContent)
    {
        return new CopyrightChapter(bookTitle, chapterTitle, chapterContent);
    }

    public void Active()
    {
        IsActive = true;
    }

    public void UnActive()
    {
        IsActive = false;
    }

    public void Update(string bookTitle,string chapterTitle, string chapterContent,
            string signatureValue,string signatureAlgorithm)
    {
        var newSignature = DigitalSignature.Create(signatureValue, signatureAlgorithm);
        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        DigitalSignature = newSignature;
    }

    public void AddSignature(string signatureAlgorithm, string signatureValue)
    {
        var signature = BookApprovalAggregate.DigitalSignature.Create(signatureValue, signatureAlgorithm);
        DigitalSignature = signature;
    }
}
