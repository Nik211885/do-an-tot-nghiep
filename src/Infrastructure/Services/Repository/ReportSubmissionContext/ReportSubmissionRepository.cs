using Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;
using Core.Interfaces.Repositories.ReportSubmissionContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.ReportSubmissionContext;

public class ReportSubmissionRepository(ReportSubmissionDbContext reportSubmissionDbContext) 
    : Repository<ReportSubmission>(reportSubmissionDbContext),
    IReportSubmissionRepository
{
    private readonly ReportSubmissionDbContext _reportSubmissionDbContext = reportSubmissionDbContext;
}
