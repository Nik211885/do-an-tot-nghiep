using Core.BoundContext.BookReviewContext.ReaderBookAggregate;
using Core.Interfaces.Repositories.BookReviewContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.BookReviewContext;

public class ReaderBookRepository(BookReviewDbContext bookReviewDbContext) 
    :   Repository<ReaderBook>(bookReviewDbContext), 
        IReaderBookRepository   
{
    public ReaderBook Create(ReaderBook reader)
    {
        return bookReviewDbContext.Add(reader).Entity;  
    }
}
