using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class ApprovalDecisionViewModel
{
    public Guid? ModeratorId { get;  }
    public DateTimeOffset DecisionDateTime { get; }
    public string? Note { get; }
    public bool IsAutomated { get; }
    public BookApprovalStatus Status { get; }

    public ApprovalDecisionViewModel(Guid? moderatorId, DateTimeOffset decisionDateTime, string? note, bool isAutomated, BookApprovalStatus status)
    {
        ModeratorId = moderatorId;
        DecisionDateTime = decisionDateTime;
        Note = note;
        IsAutomated = isAutomated;
        Status = status;
    }
}

public static class ApprovalDecisionMappingExtension
{
    public static ApprovalDecisionViewModel ToViewModel(this ApprovalDecision approvalDecision)
    {
        return new ApprovalDecisionViewModel(
            moderatorId: approvalDecision.ModeratorId,
            decisionDateTime: approvalDecision.DecisionDateTime,
            note: approvalDecision.Note,
            isAutomated: approvalDecision.IsAutomated,
            status: approvalDecision.Status
        );
    }

    public static IReadOnlyCollection<ApprovalDecisionViewModel> ToViewModel(
        this IEnumerable<ApprovalDecision> approvalDecisions)
    {
        return approvalDecisions.Select(ToViewModel).ToList();
    }
}
