using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;

namespace Core.Interfaces.Repositories.UserProfileContext;

public interface ISearchHistoryRepository
    : IRepository<SearchHistory>
{
    SearchHistory Create(SearchHistory searchHistory);
    void DeleteSearchHistory(SearchHistory searchHistory);
}
