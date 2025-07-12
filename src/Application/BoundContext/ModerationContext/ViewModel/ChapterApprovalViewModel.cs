using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class ChapterApprovalViewModel
{
    public Guid Id { get; }
    public Guid BookApprovalId { get; }
    public Guid ChapterId { get; }
    public DateTimeOffset SubmittedAt { get; }
    public string ChapterContent { get; }
    public string ChapterTitle { get; }
    public int ChapterNumber { get; }
    public string ChapterSlug { get; }
    public ChapterApprovalStatus Status { get; }
    public CopyrightChapterViewModel? CopyrightChapter { get; }
    public IReadOnlyCollection<ApprovalDecisionViewModel> Decision { get; }

    public ChapterApprovalViewModel(Guid id, Guid bookApprovalId, Guid chapterId,
        DateTimeOffset submittedAt, string chapterContent, string chapterTitle, int chapterNumber, 
        string chapterSlug, ChapterApprovalStatus status, CopyrightChapterViewModel? copyrightChapter,
        IReadOnlyCollection<ApprovalDecisionViewModel> decision)
    {
        Id = id;
        BookApprovalId = bookApprovalId;
        ChapterId = chapterId;
        SubmittedAt = submittedAt;
        ChapterContent = chapterContent;
        ChapterTitle = chapterTitle;
        ChapterNumber = chapterNumber;
        ChapterSlug = chapterSlug;
        Status = status;
        CopyrightChapter = copyrightChapter;
        Decision = decision;
    }
}

public static class ChapterApprovalMappingExtension
{
    public static ChapterApprovalViewModel ToViewModel(this ChapterApproval chapterApproval)
    {
        return new ChapterApprovalViewModel(
            id: chapterApproval.Id,
            bookApprovalId: chapterApproval.BookApprovalId,
            chapterId: chapterApproval.ChapterId,
            submittedAt: chapterApproval.SubmittedAt,
            chapterContent: chapterApproval.ChapterContent,
            chapterTitle: chapterApproval.ChapterTitle,
            chapterNumber: chapterApproval.ChapterNumber,
            chapterSlug: chapterApproval.ChapterSlug,
            status: chapterApproval.Status,
            copyrightChapter: chapterApproval.CopyrightChapter?.ToViewModel(),
            decision: chapterApproval.Decision.ToViewModel()
            );
    }
} 

    
