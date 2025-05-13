using Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class ReportSubmissionDbContext(DbContextOptions<BaseDbContext> options, IDbConnectionStringSelector dbConnectionStringSelector)
    : BaseDbContext(options, dbConnectionStringSelector)
{
    public DbSet<ReportSubmission> ReportSubmissions { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Submitter> Submitters { get; set; }
}
