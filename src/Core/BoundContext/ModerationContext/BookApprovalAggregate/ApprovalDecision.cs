using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class ApprovalDecision : BaseEntity
{
    public Guid? ModeratorId { get; private set; }
    public DateTimeOffset DecisionDateTime { get; private set; }
    public string? Note { get; private set; }
    public bool IsAutomated { get; private set; }
    public BookApprovalStatus Status { get; private set; }
    protected ApprovalDecision(){}
    private ApprovalDecision(Guid? moderatorId, string? note, bool isAutomated, BookApprovalStatus status)
    {
        ModeratorId = moderatorId;
        Status = status;
        DecisionDateTime = DateTimeOffset.UtcNow;
        Note = note;
        IsAutomated = isAutomated;
    }
    
    public static ApprovalDecision Create(Guid? moderatorId, string? note, bool isAutomated,BookApprovalStatus status)
    {
        return new ApprovalDecision(moderatorId,note, isAutomated,status);
    }
}
