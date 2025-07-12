using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class ApprovalRepositoryViewModel
{
    public Guid BookId {get;}
    public string BookTitle {get;}
    public Guid AuthorId {get;}
    public IReadOnlyCollection<CopyrightChapterRepositoryViewModel>? CopyrightChapters {get;}
    public IReadOnlyCollection<ApprovalDecisionViewModel>? ApprovalDecisions {get;}

    public ApprovalRepositoryViewModel(Guid bookId, string bookTitle,
        Guid authorId,
        IReadOnlyCollection<CopyrightChapterRepositoryViewModel>? copyrightChapters, 
        IReadOnlyCollection<ApprovalDecisionViewModel>? approvalDecisions)
    {
        BookId = bookId;
        BookTitle = bookTitle;
        AuthorId = authorId;
        CopyrightChapters = copyrightChapters;
        ApprovalDecisions = approvalDecisions;
    }
}

public class CopyrightChapterRepositoryViewModel
{
    public DateTimeOffset SubmitAt {get;}
    public string ChapterTitle {get;}
    public string ChapterSlug {get;}
    public int ChapterNumber {get;}
    public string ChapterContent {get;}
    public DateTimeOffset DateTimeCopyright {get;}

    public CopyrightChapterRepositoryViewModel(DateTimeOffset submitAt, string chapterTitle, string chapterSlug, int chapterNumber, string chapterContent, DateTimeOffset dateTimeCopyright)
    {
        ChapterTitle = chapterTitle;
        ChapterSlug = chapterSlug;
        ChapterNumber = chapterNumber;
        ChapterContent = chapterContent;
        DateTimeCopyright = dateTimeCopyright;
    }
}
