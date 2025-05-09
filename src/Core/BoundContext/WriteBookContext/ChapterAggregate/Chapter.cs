using Core.Entities;
using Core.Events.AuthoringContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.AuthoringContext.ChapterAggregate;
public class Chapter 
    : BaseEntity, IAggregateRoot
{
    public Guid BookId { get; private set; }
    private uint _currentVersion;
    public string Content { get; private set; }
    public string Title { get; private set; }
    public bool IsLocked { get; private set; }
    public ChapterStatus Status { get; private set; }
    public DateTimeOffset CreateDateTime { get; private set; }
    private List<ChapterVersion> _chapterVersions;
    public IReadOnlyCollection<ChapterVersion> ChapterVersions => _chapterVersions;
    private Chapter(Guid bookId, string content, string title)
    {
        _currentVersion = 0;
        BookId = bookId;
        Content = content;
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
            throw new BadRequestException(BookManagementContextMessage.ChapterFindBackVersion);
        }

        return _chapterVersions.Skip(chapterVersionBackIndex).ToList().AsReadOnly();
    }
    public static Chapter Create(Guid bookId, string content, string title)
    {
        return new Chapter(bookId, content, title);
    }

    /// <summary>
    ///     Please update when has change content in chapter it make new version 
    /// </summary>
    /// <param name="newContent"></param>
    /// <param name="title"></param>
    /// <param name="diffTitle"></param>
    /// <param name="diffContent"></param>
    public void UpdateChapter(string newContent, string title, string diffTitle, string diffContent)
    {
        LockedCanNotBeChanged();
        Content = newContent;
        Status = ChapterStatus.Draft;
        Title = title;
        _currentVersion++;
        var chapterVersion = ChapterVersion.Create(Id,diffTitle, diffContent,_currentVersion);
        _chapterVersions ??= [];
        _chapterVersions.Add(chapterVersion);
    }
    
    public void SubmitAndReview()
    {
        LockedCanNotBeChanged();
        Status = ChapterStatus.Submitted;
        Locked();
        RaiseDomainEvent(new SubmittedAndReviewedChapterVersionDomainEvent(Id, BookId));
    }

    private void LockedCanNotBeChanged()
    {
        if (IsLocked)
        {
            throw new BadRequestException(BookManagementContextMessage.ChapterHasLocked);
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
