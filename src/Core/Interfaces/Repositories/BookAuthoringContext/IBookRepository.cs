using Core.BoundContext.BookAuthoringContext.BookAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IBookRepository
    : IRepository<Book>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Book?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    Book Create(Book book);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="book"></param>
    /// <returns></returns>
    Book Update(Book book);
    void Delete(Book book);
}
