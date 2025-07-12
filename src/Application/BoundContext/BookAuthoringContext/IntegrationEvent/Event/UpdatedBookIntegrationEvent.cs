using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class UpdatedBookIntegrationEvent : Models.EventBus.IntegrationEvent
{
    public Book Book { get; }
    
    public UpdatedBookIntegrationEvent(Book book)
    {
        Book = book;
    }
}
