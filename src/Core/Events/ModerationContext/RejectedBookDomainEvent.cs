using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class RejectedBookDomainEvent(Guid approvalId, Guid bookId, Guid chapterId, ApprovalDecision reason)
    : IEvent
{
    public Guid ApprovalId { get; } = approvalId;
    public Guid BookId { get;} = bookId;
    public Guid ChapterId { get;} = chapterId;
    public ApprovalDecision Reason { get;} =  reason;
}
