using Core.BoundContext.AuthoringContext.ChapterAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.AuthoringContext;

public class ChapterVersionConfiguration : IEntityTypeConfiguration<ChapterVersion>
{
    public void Configure(EntityTypeBuilder<ChapterVersion> builder)
    {
        throw new NotImplementedException();
    }
}
