using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.UserProfileContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.Command.SearchHistory;

public record CleanAllSearchHistoryCommand
    : IUserProfileCommand<bool>;

public class CleanAllSearchHistoryCommandHandler(
    ILogger<CleanAllSearchHistoryCommandHandler> logger,
    ISearchHistoryRepository searchHistoryRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<CleanAllSearchHistoryCommand, bool>
{
    private readonly ILogger<CleanAllSearchHistoryCommandHandler> _logger = logger;
    private readonly ISearchHistoryRepository _searchHistoryRepository = searchHistoryRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<bool> Handle(CleanAllSearchHistoryCommand request, CancellationToken cancellationToken)
    {
        var searchHistories =
            await _searchHistoryRepository.GetAllSearchHistoriesByUserIdAsync(_identityProvider.UserIdentity(),
                cancellationToken);
        await _searchHistoryRepository.BulkDeleteAsync(searchHistories, cancellationToken);
        _logger.LogInformation("Deleted all search histories for user {@Userid}", _identityProvider.UserIdentity());
        return true;
    }
}
