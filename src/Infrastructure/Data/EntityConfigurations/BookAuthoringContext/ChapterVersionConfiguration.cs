using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class ChapterVersionConfiguration : IEntityTypeConfiguration<ChapterVersion>
{
    public void Configure(EntityTypeBuilder<ChapterVersion> builder)
    {
        builder.ToTable("ChapterVersions", DbSchema.WriteBook);
    }
}
