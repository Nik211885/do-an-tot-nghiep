using Application.Interfaces.CQRS;
using Core.Events;

namespace Application.Services.Test;

public class CreatedEventEventHandler : IDomainEventHandler<CreatedTestEvent>
{
    public async Task Publish(CreatedTestEvent domainEvent, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken);
    }
}
