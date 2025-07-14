namespace Application.BoundContext.ModerationContext.IntegrationEvent.Event;

public class ActivatedBookIntegrationEvent : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid AuthorId { get; }

    public ActivatedBookIntegrationEvent(Guid bookId, Guid authorId)
    {
        BookId = bookId;
        AuthorId = authorId;
    }
}
