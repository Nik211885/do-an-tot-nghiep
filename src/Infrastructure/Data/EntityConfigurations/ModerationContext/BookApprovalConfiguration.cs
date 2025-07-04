using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ModerationContext;

public class BookApprovalConfiguration : IEntityTypeConfiguration<BookApproval>
{
    public void Configure(EntityTypeBuilder<BookApproval> builder)
    {
        builder.ToTable("BookApprovals", DbSchema.Moderation);
        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.OwnsOne(c => c.CopyrightChapter, cp =>
        {
            cp.Property(b=>b.BookTitle)
                .HasMaxLength(200)
                .IsRequired();
            cp.Property(b => b.BookTitle)
                .HasMaxLength(200)
                .IsRequired();
            cp.OwnsOne(x => x.DigitalSignature, ds =>
            {
                ds.Property(d => d.SignatureAlgorithm)
                    .HasMaxLength(500)
                    .IsRequired();
                ds.Property(d => d.SignatureValue)
                    .HasMaxLength(500)
                    .IsRequired();
            });
        });
    }
}
