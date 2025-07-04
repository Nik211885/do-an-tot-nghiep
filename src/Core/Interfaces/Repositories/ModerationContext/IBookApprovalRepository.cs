using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.BookReviewContext;

namespace Core.Interfaces.Repositories.ModerationContext;

public interface IBookApprovalRepository
    : IRepository<BookApproval>  
{
    Task<List<BookApproval>> FindByBookIdAsync(Guid bookId, CancellationToken cancellationToken= default);
    Task<BookApproval?> FindByBookIdAndChapterIdAsync(Guid bookId, Guid chapterId, CancellationToken cancellationToken = default);
    Task<BookApproval?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    BookApproval Create(BookApproval bookApproval, CancellationToken cancellationToken = default);
    BookApproval Update(BookApproval  bookApproval, CancellationToken cancellationToken = default);
    void Delete(BookApproval bookApproval);
    Task BulkDeleteAsync(IEnumerable<BookApproval> bookApprovals, CancellationToken cancellationToken = default);
}
