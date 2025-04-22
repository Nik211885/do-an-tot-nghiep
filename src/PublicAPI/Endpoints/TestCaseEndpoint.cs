using Application.Interfaces.CQRS;
using Application.Services.Test.Command;
using Application.Services.Test.Query;
using Core.Entities.TestAggregate;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class TestCaseEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var api = endpoint.MapGroup("api/test-case");
        api.MapPost("create", TestCaseServicesEndpoints.CreateTestCase);
        api.MapGet("pagination/{PageNumber}/{PageSize}", TestCaseServicesEndpoints.GetTestCaseWithPagination);
    }
}

public static class TestCaseServicesEndpoints
{
    public static async Task<IResult> CreateTestCase([FromBody] CreateTestCommand request,
        [FromServices] IFactoryHandler factoryHandler)
    {
        var testCae = await factoryHandler.Handler<CreateTestCommand, TestCaseAggregate>(request, CancellationToken.None);
        return Results.Ok(testCae);
    }
    public static async Task<IResult> GetTestCaseWithPagination([AsParameters] GetTestCaseWithPaginationQuery request,
        [FromServices] IFactoryHandler factoryHandler)
    {
        var testCae = await factoryHandler.Handler<GetTestCaseWithPaginationQuery, PaginationItem<TestCaseAggregateResponse>>(request, CancellationToken.None);
        return Results.Ok(testCae);
    }
}
