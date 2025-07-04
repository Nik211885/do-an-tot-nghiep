using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface ISearchHistoryRepository
    : IRepository<SearchHistory>
{
    SearchHistory Create(SearchHistory searchHistory);
    void DeleteSearchHistory(SearchHistory searchHistory);
    Task<IEnumerable<SearchHistory>> GetAllSearchHistoriesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task BulkDeleteAsync(IEnumerable<SearchHistory> searchHistories, CancellationToken cancellationToken);
    Task<IEnumerable<SearchHistory>> GetSearchHistoriesByIdsAsync(CancellationToken cancellationToken, params Guid[] ids);
}
