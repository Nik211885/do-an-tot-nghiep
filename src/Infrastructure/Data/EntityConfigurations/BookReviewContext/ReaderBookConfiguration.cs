using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.BoundContext.BookReviewContext.ReaderBookAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigurations.BookReviewContext;

public class ReaderBookConfiguration
    : IEntityTypeConfiguration<ReaderBook>
{
    public void Configure(EntityTypeBuilder<ReaderBook> builder)
    {
        builder.ToTable("Reader", DbSchema.BookReview);
        builder.HasOne<BookReview>()
            .WithMany()
            .HasForeignKey(x=>x.BookReviewId);
    }
}
