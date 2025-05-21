using System.Diagnostics;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
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
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(b=>b.Slug).IsUnique();
        builder.Property(b=>b.Title).HasMaxLength(50)
            .IsRequired();
        builder.Property(b => b.AvatarUrl)
            .HasMaxLength(250);
        builder.Property(b => b.Description)
            .HasMaxLength(500);
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
            t.Property(t => t.TagName)
                .HasMaxLength(50)
                .IsRequired();
        });
    }
}
