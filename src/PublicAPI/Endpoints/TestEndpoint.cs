using Application.Interfaces.CQRS;
using Application.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace PublicAPI.Endpoints;

public class TestEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var api = endpoint.MapGroup("api/test");
        api.MapPost("/create", CreateTest);
    }

    public static async Task<IResult> CreateTest([FromBody] CreateTestCommand  request,
        [FromServices] IFactoryHandler factoryHandler)
    {
        var response = await factoryHandler.Handler<CreateTestCommand,Guid>(request, CancellationToken.None);
        return Results.Ok(response);
    }
}
