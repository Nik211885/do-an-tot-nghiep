using System.Linq.Expressions;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class ApprovalDecisionViewModel
{
    public Guid Id { get;  }
    public Guid? ModeratorId { get;  }
    public DateTimeOffset DecisionDateTime { get; }
    public string? Note { get; }
    public ChapterApprovalStatus Status { get; }

    public ApprovalDecisionViewModel(Guid id, Guid? moderatorId, DateTimeOffset decisionDateTime, string? note, ChapterApprovalStatus status)
    {
        Id = id;
        ModeratorId = moderatorId;
        DecisionDateTime = decisionDateTime;
        Note = note;
        Status = status;
    }
}

public static class ApprovalDecisionMappingExtension
{
    public static ApprovalDecisionViewModel ToViewModel(this ApprovalDecision approvalDecision)
    {
        return new ApprovalDecisionViewModel(
            id: approvalDecision.Id,
            moderatorId: approvalDecision.ModeratorId,
            decisionDateTime: approvalDecision.DecisionDateTime,
            note: approvalDecision.Note,
            status: approvalDecision.Status
        );
    }

    public static Expression<Func<ApprovalDecision, ApprovalDecisionViewModel>> ToViewModel()
    {
        return ap => ap.ToViewModel();
    }

    public static IReadOnlyCollection<ApprovalDecisionViewModel> ToViewModel(
        this IEnumerable<ApprovalDecision> approvalDecisions)
    {
        return approvalDecisions.Select(ToViewModel).ToList();
    }
}
