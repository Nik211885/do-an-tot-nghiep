using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.BookReviewContext;

namespace Core.Interfaces.Repositories.ModerationContext;

public interface IBookApprovalRepository
    : IRepository<BookApproval>  
{
    Task<BookApproval?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    BookApproval Add(BookApproval bookApproval, CancellationToken cancellationToken = default);
}
