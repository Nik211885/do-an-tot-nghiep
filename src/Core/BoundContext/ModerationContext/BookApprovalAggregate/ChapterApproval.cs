using Core.Entities;
using Core.Events.ModerationContext;
using Core.Exception;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class ChapterApproval : BaseEntity, IAggregateRoot
{
    public Guid BookApprovalId { get; private set; }
    public Guid ChapterId { get; private set; }
    public DateTimeOffset SubmittedAt { get; private set; }
    public string ChapterContent { get; private set; }
    public string ChapterTitle {get; private set;}
    public int ChapterNumber {get; private set;}
    public string ChapterSlug {get; private set;}
    public ChapterApprovalStatus Status { get; private set; }
    private List<ApprovalDecision> _decision =[];
    public IReadOnlyCollection<ApprovalDecision> Decision => _decision.AsReadOnly();
    public int Version { get; private set; }
    protected ChapterApproval(){}
    public CopyrightChapter? CopyrightChapter { get; private set; }

    private ChapterApproval(Guid bookApprovalId, Guid chapterId,string chapterContent, string chapterTitle, int chapterNumber, string chapterSlug)
    {
        BookApprovalId = bookApprovalId;
        ChapterId = chapterId;
        SubmittedAt = DateTimeOffset.UtcNow;
        ChapterContent = chapterContent;
        ChapterTitle = chapterTitle;
        ChapterNumber = chapterNumber;
        ChapterSlug = chapterSlug;
        Status = ChapterApprovalStatus.Pending;
        Version = 0;
        RaiseDomainEvent(new CreatedChapterApprovalDomainEvent(this));
    }

    public static ChapterApproval Create(Guid bookApprovalId, Guid chapterId, string chapterContent, string chapterTitle,
        int chapterNumber, string chapterSlug)
    {
        var chapterApproval = new ChapterApproval(bookApprovalId, chapterId, chapterContent, chapterTitle, chapterNumber, chapterSlug);
        return chapterApproval;
    }
    

    public void Approved(Guid moderationId, string reason)
    {
        if (Status == ChapterApprovalStatus.Approved)
        {
            ThrowHelper.ThrowIfBadRequest("Noi dung da duoc duyet");
        }
        Status = ChapterApprovalStatus.Approved;
        var decision = ApprovalDecision.Create(Id,moderationId, reason, ChapterApprovalStatus.Approved);
        _decision??=[];
        _decision.Add(decision);
        var copyrightChapter = BookApprovalAggregate.CopyrightChapter.Create(
            ChapterTitle, ChapterContent, ChapterSlug, ChapterNumber
        );
        CopyrightChapter = copyrightChapter;
        RaiseDomainEvent(new ApprovedChapterDomainEvent(this));
    }

    public void Rejected(Guid moderationId, string reason)
    {
        if (Status == ChapterApprovalStatus.Rejected)
        {
            ThrowHelper.ThrowIfBadRequest("Noi dung da bi tu choi");
        }
        Status = ChapterApprovalStatus.Rejected;
        var decision = ApprovalDecision.Create(Id,moderationId, reason, ChapterApprovalStatus.Rejected);
        _decision??=[];
        _decision.Add(decision);
        RaiseDomainEvent(new RejectedChapterDomainEvent(this));
    }

    public void OpenApproval()
    {
        this.Status = ChapterApprovalStatus.Pending;
        RaiseDomainEvent(new OpenedApprovalDomainEvent(this));
    }
}
