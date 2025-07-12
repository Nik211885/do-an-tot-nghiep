using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using EFCore.BulkExtensions;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.ModerationContext;

public class BookApprovalRepository(ModerationDbContext moderationDbContext) 
    : Repository<BookApproval>(moderationDbContext), IBookApprovalRepository
{
    private readonly ModerationDbContext _moderationDbContext = moderationDbContext;
    public async Task<BookApproval?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = _moderationDbContext
            .BookApprovals
            .Where(x => x.Id == id);
        var result = await query.FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<BookApproval?> FindByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        IQueryable<BookApproval> queryable 
            = _moderationDbContext.BookApprovals
                .Where(x=>x.BookId == bookId);
        BookApproval? result = await queryable.FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public BookApproval Create(BookApproval bookApproval)
    {
        return _moderationDbContext.BookApprovals.Add(bookApproval).Entity;
    }

    public BookApproval Update(BookApproval bookApproval)
    {
        _moderationDbContext.Entry(bookApproval).State = EntityState.Modified;
        return bookApproval;
    }

    public void Delete(BookApproval bookApproval)
    {
        _moderationDbContext.BookApprovals.Remove(bookApproval);
    }

    public async Task BulkDeleteAsync(IEnumerable<BookApproval> bookApprovals, CancellationToken cancellationToken = default)
    {
        await _moderationDbContext.BulkDeleteAsync(bookApprovals, cancellationToken: cancellationToken);
    }
}
