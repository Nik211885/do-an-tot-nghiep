using System.Reflection;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Core.BoundContext.BookReviewContext.BookReviewAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.EntityConfigurations.BookAuthoringContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class BookAuthoringDbContext(DbContextOptions<BookAuthoringDbContext> options, 
    IDbConnectionStringSelector dbConnectionStringSelector,
    DispatcherDomainEventInterceptors interceptors) 
    : BaseDbContext(options, dbConnectionStringSelector,interceptors)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<ChapterVersion> ChapterVersions { get; set; }
    public DbSet<Genres> Genres { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration())
            .ApplyConfiguration(new ChapterConfiguration())
            .ApplyConfiguration(new ChapterVersionConfiguration())
            .ApplyConfiguration(new GenreConfiguration());
    }
}
