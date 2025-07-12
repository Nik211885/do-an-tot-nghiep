using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Infrastructure.Data.EntityConfigurations.ModerationContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ModerationDbContext(DbContextOptions<ModerationDbContext> options,
    IDbConnectionStringSelector dbConnectionStringSelector,
    DispatcherDomainEventInterceptors interceptors) 
    : BaseDbContext(options, dbConnectionStringSelector, interceptors)
{
    public DbSet<BookApproval> BookApprovals { get; set; }
    public DbSet<ChapterApproval> ChapterApprovals { get; set; }
    public DbSet<ApprovalDecision>  ApprovalDecisions { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApprovalDecisionConfiguration())
            .ApplyConfiguration(new BookApprovalConfiguration())
            .ApplyConfiguration(new ChapterApprovalConfiguration());
    }
}
