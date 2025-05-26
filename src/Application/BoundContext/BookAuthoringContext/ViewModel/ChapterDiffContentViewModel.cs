namespace Application.BoundContext.BookAuthoringContext.ViewModel;

public record ChapterDiffContentViewModel(
    Guid ChapterId,
    Guid ChapterVersionId,
    string ChapterVersionName,
    string ContentPretty,
    string TitlePretty,
    DateTimeOffset LastModified,
    uint Version
);

/// <summary>
/// Data in store cache when use query and want to show different content
/// with version 
/// </summary>
/// <param name="Title"></param>
/// <param name="Content"></param>
public record ChapterRollBackData(
    string Title, 
    string Content,
    Guid ChapterId,
    Guid ChapterVersionId);
