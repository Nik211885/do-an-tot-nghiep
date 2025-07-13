using System.Linq.Expressions;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.ViewModel;

public class BookApprovalViewModel
{
    public Guid Id { get;  }
    public Guid BookId { get; }
    public Guid AuthorId { get; }
    public string BookTitle { get; }
    public bool IsActive { get; }
    public int ChapterCount { get; }

    public BookApprovalViewModel(Guid id, Guid bookId, Guid authorId, string bookTitle, bool isActive, int chapterCount)
    {
        Id = id;
        BookId = bookId;
        AuthorId = authorId;
        BookTitle = bookTitle;
        IsActive = isActive;
        ChapterCount = chapterCount;
    }
}

public static class BookApprovalViewModelMappingExtension
{
    public static BookApprovalViewModel ToViewModel(this BookApproval bookApproval)
    {
        return new BookApprovalViewModel(
           id: bookApproval.Id,
           bookId: bookApproval.BookId,
           authorId: bookApproval.AuthorId,
           bookTitle: bookApproval.BookTitle,
           isActive: bookApproval.IsActive,
           chapterCount: bookApproval.ChapterCount
        );
    }
    public static IReadOnlyCollection<BookApprovalViewModel> ToViewModel(this IEnumerable<BookApproval> bookApproval)
    {
        return bookApproval.Select(ToViewModel).ToList();
    }

    public static Expression<Func<BookApproval, BookApprovalViewModel>> ToViewModel()
    {
        return ba => new BookApprovalViewModel(
            ba.Id,
            ba.BookId,
            ba.AuthorId,
            ba.BookTitle,
            ba.IsActive,
            ba.ChapterCount
        );
    }
}

