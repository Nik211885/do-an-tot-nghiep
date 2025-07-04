using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IGenresRepository: IRepository<Genres>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> FindActiveByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    ///      Get genres by id ignore query filter with soft deleted
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///    Get list genres active by ids
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="ignore"></param>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> FindActiveByIdsAsync(CancellationToken cancellationToken,bool ignore = false , params Guid[] ids);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    Genres Create(Genres genre);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="genre"></param>
    /// <returns></returns>
    Genres Update(Genres genre);

    /// <summary>
    ///  Update bulk for genres extension for it will change data after call method
    /// </summary>
    /// <param name="genres"></param>
    /// <param name="cancellationToken"></param>
    Task UpdateBulkAsync(IReadOnlyCollection<Genres> genres, CancellationToken cancellationToken);
}   
