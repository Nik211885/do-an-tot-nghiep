using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.Validator.Chapter;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class ChapterVersionConfiguration : IEntityTypeConfiguration<ChapterVersion>
{
    public void Configure(EntityTypeBuilder<ChapterVersion> builder)
    {
        builder.ToTable("ChapterVersions", DbSchema.BookAuthoring);
        builder.Ignore(c=>c.DomainEvents);
        builder.Property(c => c.NameVersion)
            .HasMaxLength(LengthPropForChapter.MaxNameVersionChapter);
        builder.Property<Guid>("ChapterId").IsRequired();
    }
}
