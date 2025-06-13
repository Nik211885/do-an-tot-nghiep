using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.SearchHistory;

public record DeleteSearchHistoryCommand(params Guid[] SearchHistoryIds)
    : IUserProfileCommand<bool>;

public class DeleteSearchHistoryCommandHandler(
    ILogger<DeleteSearchHistoryCommandHandler> logger,
    ISearchHistoryRepository searchHistoryRepository)
    : ICommandHandler<DeleteSearchHistoryCommand, bool>
{
    private readonly ILogger<DeleteSearchHistoryCommandHandler> _logger = logger;
    private readonly ISearchHistoryRepository _searchHistoryRepository = searchHistoryRepository;
    public async Task<bool> Handle(DeleteSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        var searchHistory = await _searchHistoryRepository.GetSearchHistoriesByIdsAsync(cancellationToken, request.SearchHistoryIds);
        await _searchHistoryRepository.BulkDeleteAsync(searchHistory, cancellationToken);
        _logger.LogInformation("Deleted search history {@searchHistory}", searchHistory);
        return true;
    }
}
