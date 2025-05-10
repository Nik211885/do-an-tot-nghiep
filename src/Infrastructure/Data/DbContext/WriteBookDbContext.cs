using Core.BoundContext.WriteBookContext.BookAggregate;
using Core.BoundContext.WriteBookContext.ChapterAggregate;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class WriteBookDbContext(DbContextOptions<BaseDbContext> options, IDbConnectionStringSelector dbConnectionStringSelector) 
    : BaseDbContext(options, dbConnectionStringSelector)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<ChapterVersion> ChapterVersions { get; set; }
}
