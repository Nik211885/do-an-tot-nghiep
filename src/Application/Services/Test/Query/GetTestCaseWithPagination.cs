using Application.Helper;
using Application.Interfaces.CQRS;
using Application.Services.Test.Query;
using Core.Entities.TestAggregate;
using Core.Interfaces.Repositories;
using Core.Models;
using Riok.Mapperly.Abstractions;

namespace Application.Services.Test.Query
{
    public record GetTestCaseWithPaginationQuery(int PageNumber, int PageSize)
        : IQuery<PaginationItem<TestCaseAggregateResponse>>;


    public record TestCaseAggregateResponse(Guid Id, string Name, TestLevel TestLevel);

    public class GetTestCaseWithPaginationQueryHandler(ITestCaseRepository  testCaseRepository) 
        : IQueryHandler<GetTestCaseWithPaginationQuery, PaginationItem<TestCaseAggregateResponse>>
    {
        private readonly ITestCaseRepository _testCaseRepository = testCaseRepository;
        public async Task<PaginationItem<TestCaseAggregateResponse>> Handle(GetTestCaseWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var testCasePagination =
                await _testCaseRepository.GetTestCaseWithPaginationAsync(request.PageNumber, request.PageSize);
            var result = testCasePagination.MapTo<TestCaseAggregate, TestCaseAggregateResponse>();
            return result;
        }
    }
}


namespace Application.Helper
{
    public static partial class PaginationItemMapper
    {
        private static partial TestCaseAggregateResponse MapTo(TestCaseAggregate testCase);
    }
}
