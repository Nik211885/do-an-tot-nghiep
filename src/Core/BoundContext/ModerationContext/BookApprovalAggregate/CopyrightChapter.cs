using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class CopyrightChapter
{
    public string BookTitle { get; private set; }
    public string ChapterTitle { get; private set; }
    public string ChapterSlug { get; private set; } 
    public int ChapterNumber { get; private set; }  
    public string ChapterContent { get; private set; }
    public DigitalSignature? DigitalSignature { get; private set; }
    public DateTimeOffset DateTimeCopyright { get; private set; }
    protected CopyrightChapter(){}
    private CopyrightChapter(string bookTitle,
        string chapterTitle, string chapterContent,  string? signatureValue,
        string? signatureAlgorithm, string chapterSlug, int chapterNumber)
    {
        if (!string.IsNullOrWhiteSpace(signatureValue))
        {
            var signature = DigitalSignature.Create(signatureValue, signatureAlgorithm);
            DigitalSignature = signature;
        }

        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        ChapterSlug = chapterSlug;
        ChapterNumber = chapterNumber;
        DateTimeCopyright = DateTimeOffset.UtcNow;
    }
        
    public static CopyrightChapter Create(string bookTitle,
        string chapterTitle, string chapterContent,  string signatureValue,
        string signatureAlgorithm,string chapterSlug, int chapterNumber)
    {
        return new CopyrightChapter( bookTitle, chapterTitle, chapterContent
            , signatureValue, signatureAlgorithm, chapterSlug, chapterNumber);
    }
    public static CopyrightChapter Create(string bookTitle,
        string chapterTitle, string chapterContent,string chapterSlug, int chapterNumber)
    {
        return new CopyrightChapter( bookTitle, chapterTitle, chapterContent, null, null ,chapterSlug, chapterNumber);
    }
    
    public void AddSignature(string signatureAlgorithm, string signatureValue)
    {
        var signature = BookApprovalAggregate.DigitalSignature.Create(signatureValue, signatureAlgorithm);
        DigitalSignature = signature;
    }
}
