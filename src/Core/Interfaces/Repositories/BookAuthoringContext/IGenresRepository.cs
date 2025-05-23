using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IGenresRepository: IRepository<Genres>
{
    /// <summary>
    ///     Get all genres has active
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> FindAllActiveAsync(CancellationToken cancellationToken);
    /// <summary>
    ///     Get all genres ignore query filter with soft deleted
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> FindAllAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Get genres active by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> FindActiveBySlugAsync(string slug,CancellationToken cancellationToken);

    /// <summary>
    ///     Get genres by slug ignore query filter with soft deleted
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> FindBySlugAsync(string slug,CancellationToken cancellationToken);

    /// <summary>
    ///     Get genres active by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> FindActiveById(Guid id, CancellationToken cancellationToken);

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
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> FindActiveByIdsAsync(CancellationToken cancellationToken, params Guid[] ids);

    /// <summary>
    ///    name genres is unique and index  get genres with ignore filter
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> FindByNameAsync(string name, CancellationToken cancellationToken);
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
}   
