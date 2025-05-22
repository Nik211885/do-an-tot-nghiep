using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IGenresRepository: IRepository<Genres>
{
    /// <summary>
    ///     Get all genres has active
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> GetAllGenresActiveAsync(CancellationToken cancellationToken);
    /// <summary>
    ///     Get all genres ignore query filter with soft deleted
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> GetAllGenresAsync(CancellationToken cancellationToken);

    /// <summary>
    ///     Get genres active by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresActiveBySlugAsync(string slug,CancellationToken cancellationToken);

    /// <summary>
    ///     Get genres by slug ignore query filter with soft deleted
    /// </summary>
    /// <param name="slug"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresBySlugAsync(string slug,CancellationToken cancellationToken);

    /// <summary>
    ///     Get genres active by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresActiveById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///      Get genres by id ignore query filter with soft deleted
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    ///    Get list genres active by ids
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> GetGenresActiveByIdsAsync(CancellationToken cancellationToken, params Guid[] ids);

    /// <summary>
    ///    name genres is unique and index  get genres with ignore filter
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresByNameAsync(string name, CancellationToken cancellationToken);
}   
