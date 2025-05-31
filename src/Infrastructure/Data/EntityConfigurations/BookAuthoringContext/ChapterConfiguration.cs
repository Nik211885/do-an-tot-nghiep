using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.Validator.Chapter;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.ToTable("Chapters", DbSchema.BookAuthoring);
        builder.HasIndex(c=>c.Slug).IsUnique();
        builder.Ignore(c => c.DomainEvents);    
        builder.Property(c=>c.Slug)
            .HasMaxLength(LengthPropForChapter.SlugMaxLength)
            .IsRequired();
        builder.Property(c => c.Content)
            .IsRequired();
        builder.Property(c=>c.Title)
            .HasMaxLength(LengthPropForChapter.TitleMaxLength)
            .IsRequired();
        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(30);
        builder.HasOne<Book>()
            .WithMany()
            .HasForeignKey(c=>c.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property<uint>("_currentVersion")
            .HasColumnName("CurrentVersion");
    }
}
