using Core.BoundContext.BookReviewContext.ReaderBookAggregate;

namespace Core.Interfaces.Repositories.BookReviewContext;

public interface IReaderBookRepository
    : IRepository<ReaderBook>
{
    ReaderBook Create(ReaderBook reader);
}
