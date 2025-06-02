using Core.Entities;
using Core.Events.ModerationContext;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class BookApproval : BaseEntity, IAggregateRoot
{
    public Guid BookId { get; private set; }
    public Guid ChapterId { get; private set; }
    public Guid AuthorId { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }
    //  It just split html tag and just have save data 
    public string ContentHash { get; private set; }
    public BookApprovalStatus Status { get; private set; }
    private List<ApprovalDecision> _decision =[];
    public IReadOnlyCollection<ApprovalDecision> Decision => _decision.AsReadOnly();
    public int Version { get; private set; }
    
    public CopyrightChapter? CopyrightChapter { get; private set; }
    protected BookApproval(){}
    private BookApproval(Guid bookId, Guid chapterId, string contentHash, Guid authorId)
    {
        BookId = bookId;
        ChapterId = chapterId;
        ContentHash = contentHash;
        SubmittedAt = DateTimeOffset.UtcNow;
        Version = 0;
        AuthorId = authorId;
        Status = BookApprovalStatus.Pending;
        RaiseDomainEvent(new ChapterReadyForModerationDomainEvent(this));
    }

    public static BookApproval Create(Guid bookId, Guid chapterId, string contentHash, Guid authorId)
    {
        return new BookApproval(bookId, chapterId, contentHash, authorId);
    }

    public void Reject(Guid? moderatorId, string? note,bool isAutomated)
    {
        var decision = ApprovalDecision.Create(moderatorId, note, isAutomated, BookApprovalStatus.Rejected);
        _decision.Add(decision);
        Status = BookApprovalStatus.Rejected;
        CopyrightChapter?.UnActive();
        RaiseDomainEvent(new RejectedBookDomainEvent(this));
    }

    public void Approve(Guid? moderatorId,string? note ,bool isAutomated)
    {
        var decision = ApprovalDecision.Create(moderatorId, note, isAutomated, BookApprovalStatus.Approved);
        _decision.Add(decision);
        Status = BookApprovalStatus.Approved;
        RaiseDomainEvent(new ApprovedBookDomainEvent(this));
    }
}
