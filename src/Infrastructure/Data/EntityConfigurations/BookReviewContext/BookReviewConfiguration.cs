using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.BoundContext.BookReviewContext.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookReviewContext;

public class BookReviewConfiguration : IEntityTypeConfiguration<BookReview>
{
    public void Configure(EntityTypeBuilder<BookReview> builder)
    {
        builder.ToTable("BookReviews", DbSchema.BookReview);
    }
}
