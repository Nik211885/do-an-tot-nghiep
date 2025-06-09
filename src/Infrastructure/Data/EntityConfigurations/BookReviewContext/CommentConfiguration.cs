using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.BoundContext.BookReviewContext.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookReviewContext;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments", DbSchema.BookReview);
        builder.HasOne<BookReview>()
            .WithMany()
            .HasForeignKey(c => c.BookReviewId);
        builder.Property(c=>c.Content)
            .HasMaxLength(500)
            .IsRequired();
    }
}
