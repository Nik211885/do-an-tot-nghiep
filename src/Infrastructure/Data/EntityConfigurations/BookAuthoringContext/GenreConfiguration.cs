using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.Validator.Genre;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class GenreConfiguration : IEntityTypeConfiguration<Genres>
{
    public void Configure(EntityTypeBuilder<Genres> builder)
    {
        builder.ToTable("Genres", DbSchema.BookAuthoring);
        builder.HasQueryFilter(g => g.IsActive);
        builder.HasIndex(g=>g.Slug).IsUnique();
        builder.Ignore(g => g.DomainEvents);
        builder.Property(g=>g.Name)
            .HasMaxLength(LengthPropGenre.NameMaxLenght)
            .IsRequired();
        builder.HasIndex(g => g.Name)
            .IsUnique();
        builder.Property(g=>g.Description)
            .HasMaxLength(LengthPropGenre.DescriptionMaxLenght)
            .IsRequired();
        builder.Property(g=>g.Slug)
            .HasMaxLength(LengthPropGenre.SlugMaxLenght)
            .IsRequired();
        builder.Property(g=>g.AvatarUrl)
            .HasMaxLength(LengthPropGenre.AvatarUrlMaxLenght)
            .IsRequired();
    }
}
