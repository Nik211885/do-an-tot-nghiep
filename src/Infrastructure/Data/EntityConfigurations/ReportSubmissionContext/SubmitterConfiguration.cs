using Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ReportSubmissionContext;

public class SubmitterConfiguration : IEntityTypeConfiguration<Submitter>
{
    public void Configure(EntityTypeBuilder<Submitter> builder)
    {
        builder.ToTable("Submitters", DbSchema.ReportSubmission);
    }
}
