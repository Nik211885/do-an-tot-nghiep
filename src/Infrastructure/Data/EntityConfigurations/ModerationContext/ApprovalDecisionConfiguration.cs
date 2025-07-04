using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ModerationContext;

public class ApprovalDecisionConfiguration : IEntityTypeConfiguration<ApprovalDecision>
{
    public void Configure(EntityTypeBuilder<ApprovalDecision> builder)
    {
        builder.ToTable("ApprovalDecisions", DbSchema.Moderation);
        builder.Property<Guid>("BookApprovalsId").IsRequired();
        builder.Property(b=>b.Note)
            .HasMaxLength(500)
            .IsRequired();
        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
    }
}
