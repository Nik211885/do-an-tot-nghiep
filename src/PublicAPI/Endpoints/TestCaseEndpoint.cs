using Application.Interfaces.CQRS;
using Application.Interfaces.Query;
using Application.Models;
using Application.Services.Extend.Query.Application;
using Application.Services.Test.Command.Created;
using Core.Entities.TestAggregate;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class TestCaseEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var api = endpoint.MapGroup("api/test-case");
        api.MapPost("create", TestCaseServicesEndpoints.CreateTestCase);
        api.MapGet("pagination/{name}/{pageNumber}/{pageSize}", TestCaseServicesEndpoints.GetTestCaseWithPagination);
    }
}

public static class TestCaseServicesEndpoints
{
    public static async Task<IResult> CreateTestCase([FromBody] CreateTestCommand request,
        [FromServices] IFactoryHandler factoryHandler)
    {
        var testCae = await factoryHandler.Handler<CreateTestCommand, TestCaseAggregate>(request, CancellationToken.None);
        return TypedResults.Ok(testCae);
    }

    public static async Task<IResult> GetTestCaseWithPagination(string name, int pageNumber, int pageSize
        , [FromServices] IFactoryHandler factoryHandler)
    {
        var request = new GetTestWithPaginationQuery(name, pageNumber, pageSize);
        var result = await factoryHandler.Handler<GetTestWithPaginationQuery, PaginationItem<TestPaginationResponse>>(request, CancellationToken.None);
        return TypedResults.Ok(result);
    }
}
