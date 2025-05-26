using Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;
using Core.Interfaces.Repositories.ReportSubmissionContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ReportSubmissionDbContext(DbContextOptions<ReportSubmissionDbContext> options,
    IDbConnectionStringSelector dbConnectionStringSelector, DispatcherDomainEventInterceptors interceptors)
    : BaseDbContext(options, dbConnectionStringSelector,interceptors)
{
    public DbSet<ReportSubmission> ReportSubmissions { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Submitter> Submitters { get; set; }
}
