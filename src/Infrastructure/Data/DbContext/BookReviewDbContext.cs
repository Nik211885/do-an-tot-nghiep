using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class BookReviewDbContext(DbContextOptions<BookReviewDbContext> options
    , IDbConnectionStringSelector dbConnectionStringSelector)
    : BaseDbContext(options, dbConnectionStringSelector)
{
    
}
