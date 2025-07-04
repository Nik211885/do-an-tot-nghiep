using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.SearchHistory;

public record CreateSearchHistoryCommand(Guid UserId, string SentSearch, string IpAddress, int SearchCout)
    : IUserProfileCommand<SearchHistoryViewModel>;

public class CreateSearchHistoryCommandHandler(
    ILogger<CreateSearchHistoryCommandHandler> logger,
    ISearchHistoryRepository searchHistory)
    : ICommandHandler<CreateSearchHistoryCommand, SearchHistoryViewModel>
{
    private readonly ILogger<CreateSearchHistoryCommandHandler> _logger = logger;
    private readonly ISearchHistoryRepository _searchHistory = searchHistory;
    public async Task<SearchHistoryViewModel> Handle(CreateSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        var searchHistory = Core.BoundContext.UserProfileContext.SearchHistoryAggregate.SearchHistory.Create(request.UserId, request.SentSearch, request.IpAddress, request.SearchCout);
        _searchHistory.Create(searchHistory);
        _logger.LogInformation("Created search history has id {@Id}", searchHistory.Id);
        await _searchHistory.UnitOfWork.SaveChangeAsync(cancellationToken);
        return searchHistory.ToViewModel();
    }
}

