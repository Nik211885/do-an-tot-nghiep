using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenres>
{
    public void Configure(EntityTypeBuilder<BookGenres> builder)
    {
        builder.ToTable("BookGenres", DbSchema.BookAuthoring);
    }
}
