using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class ApprovalDecision : BaseEntity
{
    public Guid ChapterApprovalId { get; private set; }
    public Guid? ModeratorId { get; private set; }
    public DateTimeOffset DecisionDateTime { get; private set; }
    public string? Note { get; private set; }
    public ChapterApprovalStatus Status { get; private set; }
    protected ApprovalDecision(){}
    private ApprovalDecision(Guid chapterApprovalId,Guid? moderatorId, string? note, ChapterApprovalStatus status)
    {
        ModeratorId = moderatorId;
        ChapterApprovalId = chapterApprovalId;
        Status = status;
        DecisionDateTime = DateTimeOffset.UtcNow;
        Note = note;
    }
    
    public static ApprovalDecision Create(Guid chapterApprovalId, Guid? moderatorId, string? note,ChapterApprovalStatus status)
    {
        return new ApprovalDecision(chapterApprovalId, moderatorId,note,status);
    }
}
