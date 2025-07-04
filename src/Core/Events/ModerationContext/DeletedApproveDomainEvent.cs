using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class DeletedApproveDomainEvent : IEvent
{
    public BookApproval BookApproval { get; set; }

    public DeletedApproveDomainEvent(BookApproval bookApproval)
    {
        BookApproval = bookApproval;
    }
}
