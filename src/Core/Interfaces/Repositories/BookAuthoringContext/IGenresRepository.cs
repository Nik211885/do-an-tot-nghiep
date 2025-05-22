using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IGenresRepository: IRepository<Genres>
{
    /// <summary>
    ///     Get all genres has active
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> GetAllGenresActiveAsync();
    /// <summary>
    ///     Get all genres ignore query filter with soft deleted
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> GetAllGenresAsync();
    /// <summary>
    ///     Get genres active by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresActiveBySlugAsync(string slug);
    /// <summary>
    ///     Get genres by slug ignore query filter with soft deleted
    /// </summary>
    /// <param name="slug"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresBySlugAsync(string slug);
    /// <summary>
    ///     Get genres active by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresActiveById(Guid id);
    /// <summary>
    ///      Get genres by id ignore query filter with soft deleted
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Genres?> GetGenresByIdAsync(Guid id);
    /// <summary>
    ///    Get list genres active by ids
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Genres>> GetGenresActiveByIdsAsync(params Guid[] ids);
}   
