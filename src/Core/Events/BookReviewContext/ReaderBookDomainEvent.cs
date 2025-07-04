using Core.BoundContext.BookReviewContext.ReaderBookAggregate;

namespace Core.Events.BookReviewContext;

public class ReaderBookDomainEvent(ReaderBook readerBook)
    : IEvent
{
    public ReaderBook ReaderBook { get; private set; } = readerBook;
}
