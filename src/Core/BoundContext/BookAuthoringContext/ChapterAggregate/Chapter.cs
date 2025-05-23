using Core.Entities;
using Core.Events.WriteBookContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.BookAuthoringContext.ChapterAggregate;
public class Chapter 
    : BaseEntity, IAggregateRoot
{
    private uint _currentVersion;
    public Guid BookId { get; private set; }
    public string Content { get; private set; }
    public string Title { get; private set; }
    public bool IsLocked { get; private set; }
    public ChapterStatus Status { get; private set; }
    public string Slug { get; private set; }
    public DateTimeOffset CreateDateTime { get; private set; }
    private List<ChapterVersion> _chapterVersions;
    public IReadOnlyCollection<ChapterVersion> ChapterVersions => _chapterVersions;
    protected Chapter(){}
    private Chapter(Guid bookId, string content, string title, string slug)
    {
        BookId = bookId;
        _currentVersion = 0;
        Content = content;
        Slug = slug;
        Title = title;
        Status = ChapterStatus.Draft;
        IsLocked = false;
        CreateDateTime = DateTimeOffset.UtcNow;
    }
    public IReadOnlyCollection<ChapterVersion> SlideBackToVersion(uint version)
    {
        var chapterVersionBackIndex = _chapterVersions.FindIndex(x => x.Version == version);
        if (chapterVersionBackIndex < 0)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.ChapterFindBackVersion);
        }

        return _chapterVersions.Skip(chapterVersionBackIndex).ToList().AsReadOnly();
    }
    public static Chapter Create(Guid bookId, string content, string title, string slug)
    {
        return new Chapter(bookId,content, title, slug);
    }

    /// <summary>
    ///     Please update when has change content in chapter it make new version 
    /// </summary>
    /// <param name="newContent"></param>
    /// <param name="title"></param>
    /// <param name="diffTitle"></param>
    /// <param name="diffContent"></param>
    /// <param name="slug"></param>
    /// <param name="nameVersion"></param>
    public void UpdateChapter(string newContent, string title, string diffTitle, string diffContent, string slug, string? nameVersion = null)
    {
        LockedCanNotBeChanged();
        nameVersion = nameVersion ?? "Đã cập nhật";
        Content = newContent;
        Status = ChapterStatus.Draft;
        Slug = slug;
        Title = title;
        _currentVersion++;
        var chapterVersion = ChapterVersion.Create(nameVersion,diffTitle, diffContent,_currentVersion);
        _chapterVersions.Add(chapterVersion);
    }
    
    public void SubmitAndReview()
    {
        LockedCanNotBeChanged();
        Status = ChapterStatus.Submitted;
        Locked();
        RaiseDomainEvent(new SubmittedAndReviewedChapterVersionDomainEvent(Id));
    }

    private void LockedCanNotBeChanged()
    {
        if (IsLocked)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.ChapterHasLocked);
        }
    }
    public void Unlocked()
    {
        IsLocked = false;
    }

    public void Locked()
    {
        IsLocked = true;
    }
}
