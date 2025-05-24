namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public class ChapterDiffContentViewModel(
    Guid chapterId,
    Guid chapterVersionId,
    string chapterName,
    string content,
    string title,
    DateTimeOffset lastUpdatedDateTime,
    uint version
    )
{
    public Guid ChapterId { get; } = chapterId;
    public Guid ChapterVersionId { get; } = chapterVersionId;
    public string ChapterVersionName { get; } = chapterName;
    public string ContentPretty { get; } = content;
    public string TitlePretty { get; } = title;
    public DateTimeOffset LastModified { get; } = lastUpdatedDateTime;
    public uint Version { get; } = version;
};
/// <summary>
///     Data in store cache when use query and want to show different content
///     with version 
/// </summary>
/// <param name="Title"></param>
/// <param name="Content"></param>

public record ChapterRollBackData(
    string Title, 
    string Content,
    Guid ChapterId,
    Guid ChapterVersionId);
