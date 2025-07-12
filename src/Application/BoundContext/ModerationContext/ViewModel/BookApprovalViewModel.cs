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

    public BookApprovalViewModel(Guid id, Guid bookId, Guid authorId, string bookTitle, bool isActive)
    {
        Id = id;
        BookId = bookId;
        AuthorId = authorId;
        BookTitle = bookTitle;
        IsActive = isActive;
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
           isActive: bookApproval.IsActive
        );
    }
}

