using System.Text.Json.Serialization;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;


// In nature, you can't get and return data back for font end and 
// font end will calculate diff value in content and title and server will 
//  save data in database but in here I want to server is center process data
public class ChapterVersionViewModel(
    Guid id, 
    string name, 
    DateTimeOffset createdDateTime,
    string? diffTitle,
    string? diffContent,
    uint version)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public DateTimeOffset CreatedDateTime { get; } = createdDateTime;
    public string? DiffTitle { get; } = diffTitle;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DiffContent { get; } = diffContent;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public uint Version { get; } = version;
}

public class ChapterViewModel(
    Guid id,
    Guid bookId,
    string content,
    string title,
    bool isLock,
    ChapterStatus chapterStatus,
    string slug,
    DateTimeOffset createDateTime,
    IReadOnlyCollection<ChapterVersionViewModel> chapterVersions)
{
    public Guid Id { get; } = id;
    public Guid BookId { get; } = bookId;
    public string Content { get; } = content;
    public string Title { get; } = title;
    public bool IsLock { get; } = isLock;
    public ChapterStatus ChapterStatus { get; } = chapterStatus;
    public string Slug { get; } = slug;
    public DateTimeOffset CreatedDateTime { get; } = createDateTime;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyCollection<ChapterVersionViewModel> ChapterVersions { get; } = chapterVersions;
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
            createDateTime: chapter.CreateDateTime,
            chapterVersions: chapter.ChapterVersions.MapToVersionViewModel()
            );
    }

    public static ChapterVersionViewModel MapToVersionViewModel(this ChapterVersion chapterVersion)
    {
        return new ChapterVersionViewModel(
            id: chapterVersion.Id,
            name: chapterVersion.NameVersion,
            createdDateTime: chapterVersion.CreatedDateTime,
            diffTitle: chapterVersion.DiffTitle,
            diffContent: chapterVersion.DiffContent,
            version: chapterVersion.Version
        );
    }

    public static IReadOnlyCollection<ChapterVersionViewModel> MapToVersionViewModel(
        this IReadOnlyCollection<ChapterVersion> chapterVersions)
    {
        return chapterVersions.Select(MapToVersionViewModel).ToArray();
    }

    public static IReadOnlyCollection<ChapterViewModel> MapToViewModel(
        this IReadOnlyCollection<Chapter> chapters)
    {
        return chapters.Select(MapToViewModel).ToList();
    }
}

