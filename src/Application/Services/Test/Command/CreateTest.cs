using Application.Interfaces.CQRS;  
using Core.Entities.TestAggregate;
using Core.Events;
using Riok.Mapperly.Abstractions;

namespace Application.Services.Test.Command;

public record CreateTestCommand(string Name, TestLevel TestLevel) : ICommand<TestCaseAggregate>;


public class CreateTestCommandHandler(IEventDispatcher eventDispatcher) 
    : ICommandHandler<CreateTestCommand, TestCaseAggregate>
{
    private readonly IEventDispatcher _eventDispatcher = eventDispatcher;
    public async Task<TestCaseAggregate> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        var testCase = CreateTestCommandMapper.MapToTestCaseAggregate(request);
        testCase.RaiseDomainEvent(new CreatedTestCase(testCase.Id, testCase.TestLevel));
        await Task.Delay(100, cancellationToken);
        var testCaseDomainEvents = testCase.DomainEvents;
        if (testCaseDomainEvents is not null)
        {
            await _eventDispatcher.Dispatch(testCaseDomainEvents,cancellationToken);
        }
        return testCase;
    }
}


[Mapper]
public static  partial class CreateTestCommandMapper
{
    public static partial TestCaseAggregate MapToTestCaseAggregate(CreateTestCommand command);
}
