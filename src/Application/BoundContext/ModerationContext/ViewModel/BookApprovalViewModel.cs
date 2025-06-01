using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class BookApprovalViewModel
{
    public Guid Id { get;  }
    public Guid BookId { get; }
    public Guid ChapterId { get; }
    public Guid AuthorId { get; }
    public DateTimeOffset SubmittedAt { get;  }
    public string Content { get;  }
    public BookApprovalStatus Status { get; }
    public int Version { get; }
    public IReadOnlyCollection<ApprovalDecisionViewModel> DecisionViewModels { get; }
    public CopyrightChapterViewModel? CopyrightChapterViewModel { get; }

    public BookApprovalViewModel(Guid id, Guid bookId, Guid chapterId, 
        Guid authorId, DateTimeOffset submittedAt, 
        string content, BookApprovalStatus status, int version,
        IReadOnlyCollection<ApprovalDecisionViewModel> decisionViewModels,
        CopyrightChapterViewModel? copyrightChapterViewModel)
    {
        Id = id;
        BookId = bookId;
        ChapterId = chapterId;
        AuthorId = authorId;
        SubmittedAt = submittedAt;
        Content = content;
        Status = status;
        Version = version;
        DecisionViewModels = decisionViewModels;
        CopyrightChapterViewModel = copyrightChapterViewModel;
    }
}

public static class BookApprovalViewModelMappingExtension
{
    public static BookApprovalViewModel ToViewModel(this BookApproval bookApproval)
    {
        return new BookApprovalViewModel(
            id: bookApproval.Id,
            bookId: bookApproval.BookId,
            chapterId: bookApproval.ChapterId,
            authorId: bookApproval.AuthorId,
            submittedAt: bookApproval.SubmittedAt,
            content: bookApproval.ContentHash,
            status: bookApproval.Status,
            version: bookApproval.Version,
            decisionViewModels: bookApproval.Decision.ToViewModel(),
            copyrightChapterViewModel: bookApproval.CopyrightChapter?.ToViewModel()
        );
    }
}

