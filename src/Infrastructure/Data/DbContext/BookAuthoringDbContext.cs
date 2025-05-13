using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.BookAuthoringContext.ChapterAggregate;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class BookAuthoringDbContext(DbContextOptions<BaseDbContext> options, IDbConnectionStringSelector dbConnectionStringSelector) 
    : BaseDbContext(options, dbConnectionStringSelector)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<ChapterVersion> ChapterVersions { get; set; }
}
