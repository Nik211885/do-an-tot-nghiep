using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class UnActivatedBookDomainEvent(BookApproval bookApproval) : IEvent
{
    public BookApproval BookApproval { get; } = bookApproval;
}
