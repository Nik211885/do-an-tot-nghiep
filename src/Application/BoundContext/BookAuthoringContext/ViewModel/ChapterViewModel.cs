using System.Text.Json.Serialization;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public record ChapterVersionViewModel(
    Guid Id,
    string Name,
    DateTimeOffset CreatedDateTime,
    string? DiffTitle,
    string? DiffContent,
    uint Version
);

public record ChapterViewModel(
    Guid Id,
    Guid BookId,
    string Content,
    string Title,
    bool IsLock,
    ChapterStatus ChapterStatus,
    string Slug,
    int ChapterNumber,
    DateTimeOffset CreatedDateTime,
    IReadOnlyCollection<ChapterVersionViewModel>? ChapterVersions
);


public static class MappingChapterViewModelExtensions
{
    public static ChapterViewModel MapToViewModel(this Chapter chapter)
    {
        return new ChapterViewModel(
            Id: chapter.Id,
            BookId: chapter.BookId,
            Content: chapter.Content,
            Title: chapter.Title,
            IsLock: chapter.IsLocked,
            ChapterStatus: chapter.Status,
            ChapterNumber:chapter.ChapterNumber,
            Slug: chapter.Slug,
            CreatedDateTime: chapter.CreateDateTime,
            ChapterVersions: chapter.ChapterVersions.MapToVersionViewModel()
        );
    }

    public static ChapterVersionViewModel MapToVersionViewModel(this ChapterVersion chapterVersion)
    {
        return new ChapterVersionViewModel(
            Id: chapterVersion.Id,
            Name: chapterVersion.NameVersion,
            CreatedDateTime: chapterVersion.CreatedDateTime,
            DiffTitle: chapterVersion.DiffTitle,
            DiffContent: chapterVersion.DiffContent,
            Version: chapterVersion.Version
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
