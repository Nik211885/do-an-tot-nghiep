using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.Query;
using Application.Models;

namespace Application.Services.Extend.Query.Application;
// You can use some services like cache
public record GetTestWithPaginationQuery(string Name, int PageNumber, int PageSize) 
    : IQuery<PaginationItem<TestPaginationResponse>>;

public class GetTestWithPaginationQueryHandler(ITestQueryServices testQueryServices
    , ICache cache)
    : IQueryHandler<GetTestWithPaginationQuery, PaginationItem<TestPaginationResponse>>
{
    private readonly ITestQueryServices _testQueryServices = testQueryServices;
    private readonly ICache _cache = cache;
    private readonly int _cacheExpiresIn = 50;
    public async Task<PaginationItem<TestPaginationResponse>> Handle(GetTestWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var paginationItem =
            await _testQueryServices.GetTestPaginationAsync(request.Name, request.PageNumber, request.PageSize, cancellationToken);
        var key = string.Concat(PrefixCache.ApplicationPrefix, request.Name);
        await _cache.SetAsync(key, paginationItem, _cacheExpiresIn);
        return paginationItem;
    }
}
