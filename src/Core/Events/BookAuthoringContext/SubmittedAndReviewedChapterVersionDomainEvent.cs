namespace Core.Events.BookAuthoringContext;

public class SubmittedAndReviewedChapterVersionDomainEvent : IEvent
{
    public string Content { get; set; }
    public Guid ChapterVersionId { get; }
    public Guid BookId { get; }
    public Guid AuthorId { get; }

    public SubmittedAndReviewedChapterVersionDomainEvent(Guid chapterVersionId, Guid bookId, Guid authorId, string content)
    {
        Content = content;
        ChapterVersionId = chapterVersionId;
        BookId = bookId;
        AuthorId = authorId;
    }
}
