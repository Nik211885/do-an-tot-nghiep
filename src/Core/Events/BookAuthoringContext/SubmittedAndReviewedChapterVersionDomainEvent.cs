namespace Core.Events.BookAuthoringContext;

public class SubmittedAndReviewedChapterVersionDomainEvent : IEvent
{
    public string Content { get; set; }
    public Guid ChapterId{ get; }
    public string ChapterTitle { get; }
    public Guid BookId { get; }

    public SubmittedAndReviewedChapterVersionDomainEvent(Guid chapterId,
        Guid bookId, string content, string chapterTitle)
    {
        Content = content;
        ChapterId = chapterId;
        ChapterTitle = chapterTitle;
        BookId = bookId;
    }
}
