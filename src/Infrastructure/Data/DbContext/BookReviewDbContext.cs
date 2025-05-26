using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Data.Interceptors;
using Infrastructure.Services.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.DbContext;

public class BookReviewDbContext(DbContextOptions<BookReviewDbContext> options
    , IDbConnectionStringSelector dbConnectionStringSelector,
    DispatcherDomainEventInterceptors interceptors)
    : BaseDbContext(options, dbConnectionStringSelector, interceptors)
{
    
}
