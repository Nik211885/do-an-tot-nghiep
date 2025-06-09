using Application.Interfaces.CQRS;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class WriteToElasticWhenApprovedBookDomainEventHandler(
    ILogger<WriteToElasticWhenApprovedBookDomainEventHandler> logger)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly ILogger<WriteToElasticWhenApprovedBookDomainEventHandler> _logger = logger;
    public Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
