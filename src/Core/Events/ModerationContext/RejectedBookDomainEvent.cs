using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class RejectedBookDomainEvent
    : IEvent
{
    public BookApproval Approval { get; }

    public RejectedBookDomainEvent(BookApproval approval)
    {
        Approval = approval;
    }
}
