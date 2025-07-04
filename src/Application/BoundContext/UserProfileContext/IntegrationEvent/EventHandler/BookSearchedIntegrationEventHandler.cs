using Application.BoundContext.UserProfileContext.Command.SearchHistory;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Application.Models.EventBus;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.UserProfileContext.IntegrationEvent.EventHandler;

public class BookSearchedIntegrationEventHandler(
    ILogger<BookSearchedIntegrationEventHandler> logger,
    IFactoryHandler factoryHandler)
    : IIntegrationEventHandler<BookSearchedIntegrationEvent>
{
    private readonly ILogger<BookSearchedIntegrationEventHandler> _logger = logger;
    private readonly IFactoryHandler _factoryHandler = factoryHandler;
    public async Task Handle(BookSearchedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Has recived event book search integration event, {@event}", @event);
        var createSearchHistoryCommand = new CreateSearchHistoryCommand(@event.UserId, @event.StringSearch,@event.IpAddress, @event.CoutResultForSearch);
        var searchHistoryViewModel = await _factoryHandler.Handler<CreateSearchHistoryCommand, SearchHistoryViewModel>(createSearchHistoryCommand, cancellationToken);
        _logger.LogInformation("Has created search history from integration event {@Id}", searchHistoryViewModel.Id);
    }
}
