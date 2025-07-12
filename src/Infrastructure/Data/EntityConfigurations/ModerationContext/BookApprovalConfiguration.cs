using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ModerationContext;

public class BookApprovalConfiguration : IEntityTypeConfiguration<BookApproval>
{
    public void Configure(EntityTypeBuilder<BookApproval> builder)
    {
        builder.ToTable("BookApprovals", DbSchema.Moderation);
        builder.Property(x=>x.BookTitle)
            .HasMaxLength(100)
            .IsRequired();
    }
}
