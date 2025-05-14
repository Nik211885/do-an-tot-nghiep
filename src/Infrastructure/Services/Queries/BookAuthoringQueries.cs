using Application.BoundContext.BookAuthoringContext.Queries;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Queries;

public class BookAuthoringQueries(BookAuthoringDbContext bookAuthoringDbContext) 
    : IBookAuthoringQueries
{
    private readonly BookAuthoringDbContext _bookAuthoringDbContext = bookAuthoringDbContext;
}
