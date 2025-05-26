using Application.BoundContext.BookAuthoringContext.ViewModel;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Application.Helper;

public static class DiffPatchMatchHelper
{
    private static readonly diff_match_patch DiffMatchPatch = new diff_match_patch();
    public static string GetDelta(this string newText, string oldText)
    {
        var diffValue = DiffMatchPatch.diff_main(newText, oldText);
        var delta = DiffMatchPatch.diff_toDelta(diffValue);
        return delta;
    }
    public static string RollBackContent(this string newValue, string delta)
    {
        var diff = DiffMatchPatch.diff_fromDelta(newValue, delta);
        var contentRollback = DiffMatchPatch.diff_text2(diff);
        return contentRollback;
    }
    public static string PrettyContent(this string newValue, string? delta)
    {
        var diff = DiffMatchPatch.safe_diff_from_delta(newValue, delta);
        var prettyTypeHtml = DiffMatchPatch.diff_prettyHtml(diff);
        return prettyTypeHtml;
    }
    public static ChapterRollBackData ChapterVersionRollBack(this Chapter chapter, Guid chapterVersionId)
    {
        var chapterVersionNeedRollBack = chapter.GetChapterVersionMakeRollBack(chapterVersionId);
        //roll back title and content
        var (titleRollBack, contentRollBack) = chapterVersionNeedRollBack.Reverse().Aggregate(
            (title: chapter.Title, content: chapter.Content),
            (acc, c) => (
                acc.title.RollBackContent(c.DiffTitle),
                acc.content.RollBackContent( c.DiffContent)
            ));
        // save in cahe
        return new ChapterRollBackData(
            Title: titleRollBack,
            Content: contentRollBack,
            ChapterId: chapter.Id,
            ChapterVersionId: chapterVersionNeedRollBack.First().Id);
    }
}
