using Application.Interfaces.Query;

namespace Application.BoundContext.BookAuthoringContext.Validator;

public interface IBookAuthoringQueryValidator 
    : IApplicationQueryServicesExtension
{
    /// <summary>
    ///  Return true If it has exit genre has name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="compareGenreId"></param>
    /// <returns></returns>
    Task<bool> GenreBeUniqueNameAsync(string name,CancellationToken cancellationToken, Guid? compareGenreId = null);
    /// <summary>
    /// Return true if find book has id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> BookIsExitedAsync(Guid id, CancellationToken cancellationToken);
}
