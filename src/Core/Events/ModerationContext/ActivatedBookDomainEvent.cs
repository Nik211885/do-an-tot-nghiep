using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class ActivatedBookDomainEvent(BookApproval bookApproval) : IEvent
{
    public BookApproval BookApproval { get; } = bookApproval;
}
