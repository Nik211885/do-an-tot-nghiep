using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class ApprovedBookDomainEvent
    : IEvent
{
    public BookApproval Approval { get; }

    public ApprovedBookDomainEvent(BookApproval approval)
    {
        Approval = approval;
    }
}
