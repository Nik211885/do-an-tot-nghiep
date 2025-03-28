using Application.Interfaces.CQRS;  
using Core.Entities.TestAggregate;
using Core.Interfaces.Repositories;
using Riok.Mapperly.Abstractions;

namespace Application.Services.Test.Command;

public record CreateTestCommand(string Name, TestLevel TestLevel) : ICommand<TestCaseAggregate>;


public class CreateTestCommandHandler(ITestCaseRepository testCaseRepository) 
    : ICommandHandler<CreateTestCommand, TestCaseAggregate>
{
    private readonly ITestCaseRepository _testCaseRepository = testCaseRepository;
    public async Task<TestCaseAggregate> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        var testCase = CreateTestCommandMapper.MapToTestCaseAggregate(request);
        var testCaseAggregate = _testCaseRepository.CreateTestCase(testCase);
        await _testCaseRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return testCaseAggregate;
    }
}


[Mapper]
public static  partial class CreateTestCommandMapper
{
    public static partial TestCaseAggregate MapToTestCaseAggregate(CreateTestCommand command);
}
