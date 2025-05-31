using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class ApprovedBookDomainEvent(Guid approvalId, Guid bookId, Guid chapterId, ApprovalDecision decision)
    : IEvent
{
    public Guid ApprovalId { get; } = approvalId;
    public Guid BookId { get;} = bookId;
    public Guid ChapterId { get;} = chapterId;
    public ApprovalDecision Decision { get;} =  decision;
}
