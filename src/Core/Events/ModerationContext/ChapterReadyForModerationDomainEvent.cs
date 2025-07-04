using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Core.Events.ModerationContext;

public class ChapterReadyForModerationDomainEvent : IEvent
{
    public BookApproval BookApproval { get; }

    public ChapterReadyForModerationDomainEvent(BookApproval bookApproval)
    {
        BookApproval = bookApproval;
    }
}
