namespace Core.Events.BookReviewContext;

public class BookReviewCreatedDomainEvent : IEvent
{
    public Guid Id { get; }
    public Guid BookId { get; }

    public BookReviewCreatedDomainEvent(Guid id, Guid bookId)
    {
        Id = id;
        BookId = bookId;
    }
}
