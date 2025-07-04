namespace Application.BoundContext.BookReviewContext.IntegrationEvent.Event;

public class ReadeBookIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }
    public Guid CreateUserId { get; }

    public ReadeBookIntegrationEvent(Guid bookId, Guid createUserId)
    {
        BookId = bookId;
        CreateUserId = createUserId;
    }
}
