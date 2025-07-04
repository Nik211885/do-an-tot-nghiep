using Core.Entities;
using Core.Events.ModerationContext;
using Core.Exception;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class BookApproval : BaseEntity, IAggregateRoot
{
    public Guid BookId { get; private set; }
    public Guid ChapterId { get; private set; }
    public Guid AuthorId { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }
    public string ChapterContent { get; private set; }
    public string ChapterTitle {get; private set;}
    public int ChapterNumber {get; private set;}
    public string ChapterSlug {get; private set;}
    public string BookTitle {get; private set;}
    public BookApprovalStatus Status { get; private set; }
    private List<ApprovalDecision> _decision =[];
    public IReadOnlyCollection<ApprovalDecision> Decision => _decision.AsReadOnly();
    public int Version { get; private set; }
    
    public CopyrightChapter? CopyrightChapter { get; private set; }
    protected BookApproval(){}
    private BookApproval(Guid bookId, Guid chapterId, Guid authorId,
        string bookTitle, string chapterTitle, int chapterNumber, string chapterSlug,
        string chapterContent)
    {
        BookId = bookId;
        ChapterNumber = chapterNumber;
        ChapterSlug = chapterSlug;
        BookTitle = bookTitle;
        ChapterTitle = chapterTitle;
        ChapterContent = chapterContent;
        ChapterId = chapterId;
        SubmittedAt = DateTimeOffset.UtcNow;
        Version = 0;
        AuthorId = authorId;
        Status = BookApprovalStatus.Pending;
        
        RaiseDomainEvent(new ChapterReadyForModerationDomainEvent(this));
    }

    public static BookApproval Create(Guid bookId, Guid chapterId, Guid authorId,
        string bookTitle, string chapterTitle, int chapterNumber, string chapterSlug,
        string chapterContent)
    {
        return new BookApproval(bookId, chapterId, authorId, 
            bookTitle, chapterTitle, chapterNumber, 
            chapterSlug, chapterContent);
    }

    public void Reject(Guid? moderatorId, string? note,bool isAutomated)
    {
        var decision = ApprovalDecision.Create(moderatorId, note, isAutomated, BookApprovalStatus.Rejected);
        _decision.Add(decision);
        Status = BookApprovalStatus.Rejected;
        RaiseDomainEvent(new RejectedBookDomainEvent(this));
    }

    public void Approve(Guid? moderatorId,string? note ,bool isAutomated)
    {
        var decision = ApprovalDecision.Create(moderatorId, note, isAutomated, BookApprovalStatus.Approved);
        _decision.Add(decision);
        Status = BookApprovalStatus.Approved;
        CopyrightChapter = CopyrightChapter.Create(BookTitle, ChapterTitle, ChapterContent, ChapterSlug, ChapterNumber);
        RaiseDomainEvent(new ApprovedBookDomainEvent(this));
    }

    public void AddSignature(string signatureValue, string signatureAlgorithm)
    {
        if (Status != BookApprovalStatus.Approved)
        {
            ThrowHelper.ThrowIfBadRequest("Chua duyet nen khong the ki");
        }

        if (CopyrightChapter is null)
        {
            ThrowHelper.ThrowIfBadRequest("Chua co noi dung de ki");
        }
        CopyrightChapter!.AddSignature(signatureAlgorithm, signatureValue);
    }

    public void OpenApproval()
    {
        this.Status = BookApprovalStatus.Pending;
        RaiseDomainEvent(new ChapterReadyForModerationDomainEvent(this));
    }

    public void DeletedApproved()
    {
        RaiseDomainEvent(new DeletedApproveDomainEvent(this));
    }
}
