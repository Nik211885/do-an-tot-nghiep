using Core.Entities;
using Core.Events.ModerationContext;
using Core.Exception;
using Core.Interfaces;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class BookApproval : BaseEntity, IAggregateRoot
{
    public Guid BookId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string BookTitle {get; private set;}
    public bool IsActive { get; private set; }
    protected BookApproval(){}
    private BookApproval(Guid bookId, Guid authorId,
        string bookTitle)
    {
        BookId = bookId;
        BookTitle = bookTitle;
        IsActive = true;
        AuthorId = authorId;
    }

    public static BookApproval Create(Guid bookId, Guid authorId, string bookTitle)
    {
        return new BookApproval(bookId, authorId, 
            bookTitle);
    }

    public void RenameBook(string bookName)
    {
        BookTitle = bookName;
    }

    public void Active()
    {
        if (IsActive)
        {
            ThrowHelper.ThrowIfBadRequest("Da bat hoat dong cua cuon sach truoc do");
        }
        IsActive = true;
        RaiseDomainEvent(new ActivatedBookDomainEvent((this)));
    }

    public void UnActive()
    {
        if (!IsActive)
        {
            ThrowHelper.ThrowIfBadRequest("Da tat hoat dong cua cuon sach truoc do");
        }
        IsActive = false;
        RaiseDomainEvent(new UnActivatedBookDomainEvent(this));
    }
    /*public void Reject(Guid? moderatorId, string? note,bool isAutomated)
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
    }*/
}
