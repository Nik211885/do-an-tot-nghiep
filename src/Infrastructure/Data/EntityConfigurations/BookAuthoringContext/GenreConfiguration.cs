using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class GenreConfiguration : IEntityTypeConfiguration<Genres>
{
    public void Configure(EntityTypeBuilder<Genres> builder)
    {
        builder.ToTable("Genres", DbSchema.BookAuthoring);
    }
}
