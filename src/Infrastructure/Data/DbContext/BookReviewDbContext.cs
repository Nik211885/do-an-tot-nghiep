using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.BoundContext.BookReviewContext.CommentAggregate;
using Core.BoundContext.BookReviewContext.RatingAggregate;
using Core.BoundContext.BookReviewContext.ReaderBookAggregate;
using Infrastructure.Data.EntityConfigurations.BookReviewContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class BookReviewDbContext(DbContextOptions<BookReviewDbContext> options
    , IDbConnectionStringSelector dbConnectionStringSelector,
    DispatcherDomainEventInterceptors interceptors)
    : BaseDbContext(options, dbConnectionStringSelector, interceptors)
{
    public DbSet<BookReview> BookReviews { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<ReaderBook> ReaderBooks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookReviewConfiguration())
            .ApplyConfiguration(new CommentConfiguration())
            .ApplyConfiguration(new RatingConfiguration())
            .ApplyConfiguration(new ReaderBookConfiguration());
    }
}
