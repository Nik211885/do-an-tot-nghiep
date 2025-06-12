namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class DeletedBookIntegrationEvent
    : Models.EventBus.IntegrationEvent
{
    public Guid BookId { get; }

    public DeletedBookIntegrationEvent(Guid bookId)
    {
        BookId = bookId;
    }
}
