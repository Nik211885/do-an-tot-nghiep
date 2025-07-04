using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;
using Core.Interfaces.Repositories.UserProfileContext;
using EFCore.BulkExtensions;
using Infrastructure.Data.DbContext;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<SearchHistory>> GetAllSearchHistoriesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var searchHistories = await _userProfileDbContext.SearchHistories
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        return searchHistories;
    }

    public async Task BulkDeleteAsync(IEnumerable<SearchHistory> searchHistories, CancellationToken cancellationToken)
    { 
        await _userProfileDbContext.BulkDeleteAsync(searchHistories, cancellationToken:cancellationToken);
    }

    public async Task<IEnumerable<SearchHistory>> GetSearchHistoriesByIdsAsync(CancellationToken cancellationToken, params Guid[] ids)
    {
        var searchHistory
            = await _userProfileDbContext.SearchHistories
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(cancellationToken);
        return searchHistory;
    }
}
