using Application.Interfaces.CQRS;
using Application.Services.Test.Command;
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
}
