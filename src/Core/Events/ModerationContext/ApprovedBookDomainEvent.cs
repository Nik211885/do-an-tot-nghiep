namespace Core.Events.ModerationContext;

public class ApprovedBookDomainEvent(Guid approvalId, Guid bookId, Guid chapterId, string? note)
    : IEvent
{
    public Guid ApprovalId { get; } = approvalId;
    public Guid BookId { get;} = bookId;
    public Guid ChapterId { get;} = chapterId;
    public string? Note { get;} =  note;
}
