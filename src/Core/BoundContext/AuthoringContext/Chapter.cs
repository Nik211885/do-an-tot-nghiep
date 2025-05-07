using Core.Entities;
using Core.Exception;
using Core.Message;

namespace Core.BoundContext.BookManagement.BookAggregate;
/// <summary>
/// 
/// </summary>
public class Chapter : BaseEntity
{
    /// <summary>
    /// 
    /// </summary>
    private Guid _bookId;
    /// <summary>
    /// 
    /// </summary>
    public Book Book { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; private set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public int ChapterNumber { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset LastUpdateDateTime { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTimeOffset PublishedDateTime { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? DraftContent { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PublishedContent { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsLocked { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ChapterSummary { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    ///
    public string? ReviewNotes { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public ChapterStatus Status { get; private set; }

    private Chapter(Guid bookId, string title, int chapterNumber, string draftContent, string? summary)
    {
        _bookId = bookId;
        Title = title;
        ChapterNumber = chapterNumber;
        DraftContent = draftContent;
        PublishedDateTime = DateTimeOffset.UtcNow;
        IsLocked = false;
        ChapterSummary = summary;
        CreatedDateTime = DateTimeOffset.UtcNow;
        Status = ChapterStatus.Draft;
    }

    public static Chapter Create(Guid bookId, string title, int chapterNumber, string draftContent,
        string? summary = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BadRequestException(BookManagementContextMessage.ChapterTitleCanNotNull);
        }

        if (chapterNumber <= 0)
        {
            throw new BadRequestException(BookManagementContextMessage.ChapterNumberCanNotBeImPositive);
        }
        return new Chapter(bookId, title, chapterNumber, draftContent, summary);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newContent"></param>
    /// <exception cref="BadRequestException"></exception>
    public void UpdateDraftContent(string newContent)
    {
        CheckLock();
        DraftContent = newContent;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>

    internal void LockEditing()
    {
        IsLocked = true;
    }
    /// <summary>
    /// 
    /// </summary>

    internal void UnlockEditing()
    {
        IsLocked = false;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newTitle"></param>
    /// <exception cref="BadRequestException"></exception>

    public void UpdateTitle(string newTitle)
    {
        CheckLock();
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            throw new BadRequestException(BookManagementContextMessage.ChapterTitleCanNotNull);
        }
        Title = newTitle;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newSummary"></param>

    public void UpdateSummary(string newSummary)
    {
        CheckLock();
        ChapterSummary = newSummary;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="BadRequestException"></exception>
    ///
    public void UpdateChapterNumber(int newChapterNumber)
    {
        CheckLock();
        if (newChapterNumber <= 0)
        {
            throw new BadRequestException(BookManagementContextMessage.ChapterNumberCanNotBeImPositive);
        }
        ChapterNumber = newChapterNumber;
        LastUpdateDateTime = DateTimeOffset.UtcNow;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newTitle"></param>
    /// <param name="newDraftContent"></param>
    /// <param name="newSummary"></param>
    /// <param name="chapterNumber"></param>
    public void Update(string newTitle, string newDraftContent, string newSummary, int chapterNumber)
    {
        CheckLock();
        UpdateTitle(newTitle);
        UpdateDraftContent(newDraftContent);
        UpdateSummary(newSummary);
        UpdateChapterNumber(chapterNumber);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="BadRequestException"></exception>
    private void CheckLock()
    {
        if (IsLocked)
        {
            throw new BadRequestException(BookManagementContextMessage.ChapterHasLocked);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="BadRequestException"></exception>

    public void SubmitForReview()
    {
        CheckLock();
        if (Status != ChapterStatus.Draft)
        {
            throw new BadRequestException(BookManagementContextMessage.CanNotPublishChapterBecauseChapterDontInDraftOrPeddingReview);
        }
        
        if (DraftContent is null)
        {
            throw new BadRequestException(BookManagementContextMessage.CanNotPublishChapterContentEmpty);
        }
        Status = ChapterStatus.Submitted;
        LockEditing();
    }
}
