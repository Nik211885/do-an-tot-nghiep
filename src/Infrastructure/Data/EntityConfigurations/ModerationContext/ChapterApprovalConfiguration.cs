using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.ModerationContext;

public class ChapterApprovalConfiguration : IEntityTypeConfiguration<ChapterApproval>
{
    public void Configure(EntityTypeBuilder<ChapterApproval> builder)
    {
        builder.ToTable("ChapterApproval", DbSchema.Moderation);
        builder.HasOne<BookApproval>()
            .WithMany()
            .HasForeignKey(x => x.BookApprovalId);
        builder.Property(x=>x.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();
        builder.Property(x=>x.ChapterContent)
            .IsRequired();
        builder.Property(x=>x.ChapterSlug)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x=>x.ChapterTitle)
            .HasMaxLength(100)
            .IsRequired();
        builder.OwnsOne(x => x.CopyrightChapter, c =>
        {
            c.Property(x => x.ChapterContent);
            c.Property(x=>x.ChapterTitle)
                .HasMaxLength(100);
            c.Property(x=>x.ChapterSlug)
            .HasMaxLength(100);
        });
        builder.HasMany(x => x.Decision)
            .WithOne()
            .HasForeignKey(x => x.ChapterApprovalId);
    }
}
