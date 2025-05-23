using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;


public class ChapterViewModel(
    Guid id,
    Guid bookId,
    string content,
    string title,
    bool isLock,
    ChapterStatus chapterStatus,
    string slug,
    DateTimeOffset createDateTime)
{
    public Guid Id { get; } = id;
    public Guid BookId { get; } = bookId;
    public string Content { get; } = content;
    public string Title { get; } = title;
    public bool IsLock { get; } = isLock;
    public ChapterStatus ChapterStatus { get; } = chapterStatus;
    public string Slug { get; } = slug;
    public DateTimeOffset CreatedDateTime { get; } = createDateTime;
}

public static class MappingChapterViewModelExtensions
{
    public static ChapterViewModel MapToViewModel(this Chapter chapter)
    {
        return new ChapterViewModel(
            id: chapter.Id,
            bookId: chapter.BookId,
            content: chapter.Content,
            title: chapter.Title,
            isLock:chapter.IsLocked,
            chapterStatus: chapter.Status,
            slug: chapter.Slug,
            createDateTime: chapter.CreateDateTime
            );
    }
}
