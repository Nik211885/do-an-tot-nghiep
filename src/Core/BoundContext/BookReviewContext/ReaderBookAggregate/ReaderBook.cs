using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Interfaces;

namespace Core.BoundContext.BookReviewContext.ReaderBookAggregate;

public class ReaderBook : BaseEntity, IAggregateRoot
{
    public Guid BookReviewId { get; private set; }
    public Guid UserId { get; private set; }
    /*public Guid ChapterId { get; private set; }*/
    // Add process reader
    public DateTimeOffset ReaderAt { get;  private set; }

    private ReaderBook(Guid bookReviewId, Guid userId)
    {
        BookReviewId = bookReviewId;
        UserId = userId;
        /*ChapterId = chapterId;*/
        ReaderAt = DateTimeOffset.UtcNow;
    }

    public static ReaderBook Create(Guid bookReviewId, Guid userId)
    {
        var reader = new ReaderBook(bookReviewId, userId); 
        reader.RaiseDomainEvent(new ReaderBookDomainEvent(reader));
        return reader;
    }
}
