using Application.Interfaces.CQRS;
using Core.Events;

namespace Application.Services.Test.Command;

public class CreatedTestCaseEventHandler : IEventHandler<CreatedTestCase>
{
    public async Task Handler(CreatedTestCase domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("Created event handler");
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine("Created event handler");
    }
}

public class CreateTestCaseEventHandlerV2 : IEventHandler<CreatedTestCase>
{
    public async Task Handler(CreatedTestCase domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine("Created event handler v2");
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine("Created event handler v2");
    }
}
