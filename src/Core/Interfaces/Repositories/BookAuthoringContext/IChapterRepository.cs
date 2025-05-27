using Core.BoundContext.BookAuthoringContext.ChapterAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IChapterRepository
    : IRepository<Chapter>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Chapter?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chapter"></param>
    /// <returns></returns>
    Chapter Create(Chapter chapter);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="chapter"></param>
    /// <returns></returns>
    Chapter Update(Chapter chapter);
    /// <summary>
    ///     Delete chapter
    /// </summary>
    /// <param name="chapter"></param>
    /// <returns></returns>
   void Delete(Chapter chapter);
}
