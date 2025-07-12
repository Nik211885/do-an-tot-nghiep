using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.BookReviewContext;

namespace Core.Interfaces.Repositories.ModerationContext;

public interface IBookApprovalRepository
    : IRepository<BookApproval>
{
    Task<BookApproval?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BookApproval?> FindByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
    BookApproval Create(BookApproval bookApproval);
    BookApproval Update(BookApproval  bookApproval);
    void Delete(BookApproval bookApproval);
    Task BulkDeleteAsync(IEnumerable<BookApproval> bookApprovals, CancellationToken cancellationToken = default);
}
