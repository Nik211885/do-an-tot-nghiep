using Core.Entities;
using Core.ValueObjects;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class ApprovalDecision : ValueObject
{
    public Guid? ModeratorId { get; private set; }
    public DateTimeOffset DecisionDateTime { get; private set; }
    public string? Note { get; private set; }
    public bool IsAutomated { get; private set; }

    private ApprovalDecision(Guid? moderatorId, string? note, bool isAutomated)
    {
        ModeratorId = moderatorId;
        DecisionDateTime = DateTimeOffset.UtcNow;
        Note = note;
        IsAutomated = isAutomated;
    }

    public void UpdateNote(string? note)
    {
        Note = note;
    }
    public static ApprovalDecision Create(Guid? moderatorId, string? note, bool isAutomated)
    {
        return new ApprovalDecision(moderatorId,note, isAutomated);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        if (ModeratorId is not null)
        {
            yield return ModeratorId;
        }

        yield return DecisionDateTime;
        yield return Note;
        yield return IsAutomated;
    }
}
