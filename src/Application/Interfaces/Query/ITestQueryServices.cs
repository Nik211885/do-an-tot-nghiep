using Application.Models;

namespace Application.Interfaces.Query;

public interface ITestQueryServices : IApplicationQueryServicesExtension
{
    Task<PaginationItem<TestPaginationResponse>> GetTestPaginationAsync(string name, int  pageNumber, int pageSize, CancellationToken cancellationToken);
}

public record TestPaginationResponse(Guid Id, string Name);
