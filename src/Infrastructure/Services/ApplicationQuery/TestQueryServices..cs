using Application.Interfaces.Query;
using Application.Models;
using Core.Entities.TestAggregate;
using Infrastructure.Data;
using Infrastructure.Helper;

namespace Infrastructure.Services.ApplicationQuery;

public class TestQueryServices(ApplicationDbContext dbContext) : ITestQueryServices
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<PaginationItem<TestPaginationResponse>> GetTestPaginationAsync(string name, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var testPaginationQueryable = _dbContext.TestCaseAggregate.AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
        {
            testPaginationQueryable = testPaginationQueryable.Where(x => x.Name.ToLower().Contains(name.ToLower()));
        }

        var testPaginationResult = await testPaginationQueryable
            .CreatePaginationAsync<TestCaseAggregate, TestPaginationResponse>(entity=> 
                new TestPaginationResponse(entity.Id, entity.Name), pageNumber, pageSize, cancellationToken: cancellationToken);
        return testPaginationResult;
        
    }
}
