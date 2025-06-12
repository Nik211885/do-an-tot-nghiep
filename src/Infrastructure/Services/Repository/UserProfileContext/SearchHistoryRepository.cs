using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using Infrastructure.Data.DbContext;

namespace Infrastructure.Services.Repository.UserProfileContext;

public class SearchHistoryRepository(UserProfileDbContext dbContext) : Repository<SearchHistory>(dbContext),  ISearchHistoryRepository
{
    private readonly UserProfileDbContext _userProfileDbContext = dbContext;
    public SearchHistory Create(SearchHistory searchHistory)
    {
        return _userProfileDbContext.SearchHistories.Add(searchHistory).Entity;
    }

    public void DeleteSearchHistory(SearchHistory searchHistory)
    {
        _userProfileDbContext.SearchHistories.Remove(searchHistory);
    }
}
