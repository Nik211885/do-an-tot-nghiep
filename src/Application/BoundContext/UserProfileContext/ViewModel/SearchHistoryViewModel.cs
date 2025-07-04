using Core.BoundContext.UserProfileContext.SearchHistoryAggregate;

namespace Application.BoundContext.UserProfileContext.ViewModel;

public class SearchHistoryViewModel
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public string SearchTerm { get; }
    public string IpAddress { get; }
    public int CoutResultForSearch { get; }
    public DateTimeOffset Created { get; }

    public SearchHistoryViewModel(Guid id, Guid userId, string searchTerm, DateTimeOffset created,  string ipAddress, int coutResultForSearch)
    {
        Id = id;
        UserId = userId;
        SearchTerm = searchTerm;
        Created = created;
        IpAddress = ipAddress;
        CoutResultForSearch = coutResultForSearch;
    }
}

public static class MappingSearchHistoryExtensions
{
    public static SearchHistoryViewModel ToViewModel(this SearchHistory searchHistory)
    {
        return new SearchHistoryViewModel(
                id: searchHistory.Id,
                userId: searchHistory.UserId,
                searchTerm: searchHistory.SearchTerm,
                created: searchHistory.SearchDate,
                ipAddress: searchHistory.IpAddress,
                coutResultForSearch: searchHistory.SearchCout
            );
    }

    public static IEnumerable<SearchHistoryViewModel> ToViewModel(this IEnumerable<SearchHistory> searchHistory)
    {
        return searchHistory.Select(ToViewModel);
    }
}
