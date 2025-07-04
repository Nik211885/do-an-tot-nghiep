using Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ReportSubmissionContext;

public class ReportSubmissionConfiguration : IEntityTypeConfiguration<ReportSubmission>
{
    public void Configure(EntityTypeBuilder<ReportSubmission> builder)
    {
        builder.ToTable("ReportSubmission", DbSchema.ReportSubmission);
    }
}
