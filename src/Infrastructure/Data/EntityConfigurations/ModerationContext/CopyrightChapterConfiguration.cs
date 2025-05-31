using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ModerationContext;

public class CopyrightChapterConfiguration : IEntityTypeConfiguration<CopyrightChapter>
{
    public void Configure(EntityTypeBuilder<CopyrightChapter> builder)
    {
        builder.ToTable("CopyrightChapters", DbSchema.Moderation);
        builder.Property(c=>c.BookTitle)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(c=>c.ChapterTitle)
            .HasMaxLength(100)
            .IsRequired();
        builder.OwnsOne(c => c.DigitalSignature, ds =>
        {
            ds.Property(d=>d.SignatureAlgorithm)
                .HasMaxLength(500)
                .IsRequired();
            ds.Property(d=>d.SignatureValue)
                .HasMaxLength(500)
                .IsRequired();
        });
    }
}
