namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class CreatedChapterApprovalIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid ChapterId { get; }
    public Guid AuthorId { get; }

    public CreatedChapterApprovalIntegrationEvent(Guid bookId, Guid chapterId, Guid authorId)
    {
        BookId = bookId;
        ChapterId = chapterId;
        AuthorId = authorId;
    }
}
