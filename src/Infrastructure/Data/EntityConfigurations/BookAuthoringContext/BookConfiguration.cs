
using Application.BoundContext.BookAuthoringContext.Message;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookAuthoringContext;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books", DbSchema.BookAuthoring);
        builder.Ignore(b => b.DomainEvents);
        builder.Property(b=>b.Slug)
            .HasMaxLength(LengthPropForBook.SlugMaxLenght)
            .IsRequired();
        builder.HasIndex(b=>b.Slug).IsUnique();
        builder.Property(b=>b.Title).HasMaxLength(50)
            .IsRequired();
        builder.Property(b => b.AvatarUrl)
            .HasMaxLength(LengthPropForBook.AvatarUrlMaxLenght);
        builder.Property(b => b.Description)
            .HasMaxLength(LengthPropForBook.DescriptionMaxLenght);
        builder.OwnsOne(b => b.PolicyReadBook, policy =>
        {
            policy.Property(p => p.Policy)
                .HasConversion<string>()
                .HasMaxLength(30);
        });
        builder.Property(b => b.BookReleaseType)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.OwnsMany(b => b.Tags, t =>
        {
            t.WithOwner().HasForeignKey("BookId");
            t.Property<int>("Id");
            t.HasKey("Id");
            t.ToTable("BookTags", DbSchema.BookAuthoring);
            t.Property(tag => tag.TagName)
                .HasMaxLength(LengthPropForBook.TagNameMaxLenght)
                .IsRequired();
        });
        builder.OwnsMany<BookGenres>(b => b.Genres,
            bg =>
            {
                const string bookFk = "BookId";
                bg.ToTable("BookGenres", DbSchema.BookAuthoring);
                bg.WithOwner().HasForeignKey(bookFk);
                bg.Property(x => x.GenreId).IsRequired();
                bg.HasKey(bookFk,nameof(BookGenres.GenreId));
                bg.HasOne<Genres>()
                    .WithMany()
                    .HasForeignKey(nameof(BookGenres.GenreId));
            });
    }
}
