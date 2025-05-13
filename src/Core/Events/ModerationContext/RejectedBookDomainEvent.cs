namespace Core.Events.ModerationContext;

public class RejectedBookDomainEvent(Guid approvalId, Guid bookId, Guid chapterId, string reason)
    : IEvent
{
    public Guid ApprovalId { get; } = approvalId;
    public Guid BookId { get;} = bookId;
    public Guid ChapterId { get;} = chapterId;
    public string Reason { get;} =  reason;
}
