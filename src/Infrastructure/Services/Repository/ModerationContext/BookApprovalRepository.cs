using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository.ModerationContext;

public class BookApprovalRepository(ModerationDbContext moderationDbContext) 
    : Repository<BookApproval>(moderationDbContext), IBookApprovalRepository
{
    private readonly ModerationDbContext _moderationDbContext = moderationDbContext;
    public async Task<BookApproval?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bookApproval = await _moderationDbContext.BookApprovals
            .FirstOrDefaultAsync(x=>x.Id == id, cancellationToken);
        if (bookApproval is not null)
        {
            await _moderationDbContext.Entry(bookApproval)
                .Collection(x=>x.Decision)
                .LoadAsync(cancellationToken);
        }
        return bookApproval;
    }

    public BookApproval Create(BookApproval bookApproval, CancellationToken cancellationToken = default)
    {
        return _moderationDbContext.BookApprovals.Add(bookApproval).Entity;
    }

    public BookApproval Update(BookApproval bookApproval, CancellationToken cancellationToken = default)
    {
        _moderationDbContext.Entry(bookApproval).State = EntityState.Modified;
        return bookApproval;
    }
}
