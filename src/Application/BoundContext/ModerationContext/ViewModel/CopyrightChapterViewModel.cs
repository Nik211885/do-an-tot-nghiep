using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class CopyrightChapterViewModel
{
    public string BookTitle { get; }
    public string ChapterTitle { get; }
    public string ChapterSlug { get; }  
    public int ChapterNumber { get; }
    public string ChapterContent { get; }
    public DateTimeOffset DateTimeCopyright { get; }
    public DigitalSignatureViewModel? DigitalSignature { get; }

    public CopyrightChapterViewModel(string bookTitle, string chapterTitle, 
        string chapterContent,
        DateTimeOffset dateTimeCopyright, 
        string chapterSlug, int chapterNumber,
        DigitalSignatureViewModel? digitalSignature)
    {
        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterSlug = chapterSlug;
        ChapterNumber = chapterNumber;
        ChapterContent = chapterContent;
        DateTimeCopyright = dateTimeCopyright;
        DigitalSignature = digitalSignature;
    }
}

public class DigitalSignatureViewModel
{
    public string SignatureValue { get; }
    public string SignatureAlgorithm { get; }
    public DateTimeOffset SigningDateTime { get; }

    public DigitalSignatureViewModel(string signatureValue, string signatureAlgorithm, DateTimeOffset signingDateTime)
    {
        SignatureValue = signatureValue;
        SignatureAlgorithm = signatureAlgorithm;
        SigningDateTime = signingDateTime;
    }
}


public static class CopyrightChapterMappingExtension
{
    public static CopyrightChapterViewModel ToViewModel(this CopyrightChapter copyrightChapter)
    {
        var signature = copyrightChapter.DigitalSignature;
        DigitalSignatureViewModel? signatureViewModel = null;
        if (signature is not null)
        {
            signatureViewModel = new DigitalSignatureViewModel(
                signatureValue: signature.SignatureValue,
                signatureAlgorithm: signature.SignatureAlgorithm,
                signingDateTime: signature.SigningDateTime
            );
        }

        return new CopyrightChapterViewModel(
            bookTitle: copyrightChapter.BookTitle,
            chapterTitle: copyrightChapter.ChapterTitle,
            chapterContent: copyrightChapter.ChapterContent,
            dateTimeCopyright: copyrightChapter.DateTimeCopyright,
            chapterSlug:  copyrightChapter.ChapterSlug,
            chapterNumber: copyrightChapter.ChapterNumber,
            digitalSignature: signatureViewModel
        );
    }
}
    
