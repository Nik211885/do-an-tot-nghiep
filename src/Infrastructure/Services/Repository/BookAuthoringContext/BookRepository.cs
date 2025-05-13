using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.BookAuthoringContext;

public class BookRepository(BookAuthoringDbContext bookAuthoringDbContext) 
    : Repository<Book>(bookAuthoringDbContext), IBookRepository
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
}
