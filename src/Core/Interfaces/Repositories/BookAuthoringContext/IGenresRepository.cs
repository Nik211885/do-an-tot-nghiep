using Core.BoundContext.BookAuthoringContext.GenresAggregate;

namespace Core.Interfaces.Repositories.BookAuthoringContext;

public interface IGenresRepository : IRepository<Genres>
{
    Task<Genres?> FindByIdAsync(Guid id, bool isActive= true);
    Task<IReadOnlyCollection<Genres>> GetAllGenresAsync();
    Task<IReadOnlyCollection<Genres>> GetAllGenresAsync(bool isActive);
}
