using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.BoundContext.BookReviewContext.RatingAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookReviewContext;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings", DbSchema.BookReview);
        builder.OwnsOne(r => r.Star, s =>
        {
        });
        builder.HasOne<BookReview>()
            .WithMany()
            .HasForeignKey(c => c.BookReviewId);
    }
}
