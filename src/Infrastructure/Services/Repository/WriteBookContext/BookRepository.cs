using Core.BoundContext.WriteBookContext.BookAggregate;
using Core.Interfaces.Repositories.WriteBookContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.WriteBookContext;

public class BookRepository(WriteBookDbContext writeBookDbContext) 
    : Repository<Book>(writeBookDbContext), IBookRepository
{
    private readonly WriteBookDbContext _writeBookDbContext = writeBookDbContext;
}
