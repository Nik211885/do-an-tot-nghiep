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
    public string ContentHash { get; private set; }
    public BookApprovalStatus Status { get; private set; }
    public ApprovalDecision Decision { get; private set; }
    public int Version { get; private set; }
    
    public CopyrightChapter? CopyrightChapter { get; private set; }
    protected BookApproval(){}
    private BookApproval(Guid bookId, Guid chapterId, string contentHash, Guid? moderatorId, string? note,
        bool isAutomated, Guid authorId)
    {
        BookId = bookId;
        ChapterId = chapterId;
        ContentHash = contentHash;
        var approvalDecision = ApprovalDecision.Create(moderatorId, note, isAutomated);
        Decision = approvalDecision;
        SubmittedAt = DateTimeOffset.UtcNow;
        Version = 0;
        AuthorId = authorId;
        Status = BookApprovalStatus.Pending;
    }

    public static BookApproval ModerationByModerator(Guid bookId, Guid chapterId, string contentHash, Guid moderatorId,
        string? note, Guid authorId)
    {
        return new BookApproval(bookId,chapterId, contentHash, moderatorId, note, false, authorId);
    }

    public static BookApproval ModerationByAutomatic(Guid bookId, Guid chapterId, string contentHash, string? note, Guid authorId)
    {
        return new BookApproval(bookId, chapterId, contentHash, null, note, true, authorId);
    }

    public void Reject(string note)
    {
        Decision.UpdateNote(note);
        Status = BookApprovalStatus.Rejected;
        if (Decision.Note is null)
        {
            throw new System.Exception();
        }
        CopyrightChapter?.UnActive();
        RaiseDomainEvent(new RejectedBookDomainEvent(Id, BookId, ChapterId, Decision.Note));
    }

    public void Approve(string bookTitle,string chapterTitle, string chapterContent, string chapterContentPlainText,
        string signatureValue,string signatureAlgorithm,string? note = null)
    {
        Decision.UpdateNote(note);
        Status = BookApprovalStatus.Approved;
        if (CopyrightChapter is null)
        {
            CopyrightChapter = BookApprovalAggregate.CopyrightChapter.Create(AuthorId, BookId, ChapterId, bookTitle,
                chapterTitle, chapterContent, chapterContentPlainText, signatureValue, signatureAlgorithm);
        }
        else
        {
            CopyrightChapter.Update(bookTitle, chapterTitle, chapterContent, chapterContentPlainText, signatureValue,
                signatureAlgorithm);
        }
        RaiseDomainEvent(new ApprovedBookDomainEvent(Id, BookId, ChapterId, Decision.Note));
    }
}
