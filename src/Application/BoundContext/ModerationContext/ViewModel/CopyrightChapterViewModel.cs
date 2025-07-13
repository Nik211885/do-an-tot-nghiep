using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class CopyrightChapterViewModel
{
    public string ChapterTitle { get; }
    public string ChapterSlug { get; }  
    public int ChapterNumber { get; }
    public string ChapterContent { get; }
    public DateTimeOffset DateTimeCopyright { get; }

    public CopyrightChapterViewModel(string chapterTitle, 
        string chapterContent,
        DateTimeOffset dateTimeCopyright, 
        string chapterSlug, int chapterNumber)
    {
        ChapterTitle = chapterTitle;
        ChapterSlug = chapterSlug;
        ChapterNumber = chapterNumber;
        ChapterContent = chapterContent;
        DateTimeCopyright = dateTimeCopyright;
    }
}

public static class CopyrightChapterMappingExtension
{
    public static CopyrightChapterViewModel ToViewModel(this CopyrightChapter copyrightChapter, bool ignoreContent = true)
    {
        return new CopyrightChapterViewModel(
            chapterTitle: copyrightChapter.ChapterTitle,
            chapterContent: ignoreContent ? string.Empty : copyrightChapter.ChapterContent,
            dateTimeCopyright: copyrightChapter.DateTimeCopyright,
            chapterSlug:  copyrightChapter.ChapterSlug,
            chapterNumber: copyrightChapter.ChapterNumber
        );
    }
}
    
