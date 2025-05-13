using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.ModerationContext;

public class BookApprovalRepository(ModerationDbContext moderationDbContext) 
    : Repository<BookApproval>(moderationDbContext), IBookApprovalRepository
{
    private ModerationDbContext _moderationDbContext = moderationDbContext;
}
