using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Application.BoundContext.BookAuthoringContext.IntegrationEvent.Event;

public class CreatedBookIntegrationEvent(Book book) : Models.EventBus.IntegrationEvent
{
    public Book Book { get; } = book;
}
