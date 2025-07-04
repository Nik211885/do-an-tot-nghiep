using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class ChapterStoreViewModel(string chapterTitle, 
    string chapterSlug, 
    int chapterNumber)
{
    public string ChapterTitle { get;  } = chapterTitle;
    public string ChapterSlug { get; } = chapterSlug;
    public int ChapterNumber { get;  } = chapterNumber;
}
