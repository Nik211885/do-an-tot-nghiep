using Application.Interfaces.CQRS;
using Application.Services.Extend.Query.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class ExtendEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("api/extend");
        apis.MapPost("get-url-upload-file", ExtendEndpointService.GetUrlUploadFileWithSignature);
    }
}

public static class ExtendEndpointService
{
    [Authorize]
    public static async Task<IResult> GetUrlUploadFileWithSignature([FromServices] IFactoryHandler factoryHandler)
    {
        var query = new GetUploadFileUrlWithSignatureQuery();
        var url = await factoryHandler.Handler<GetUploadFileUrlWithSignatureQuery, string>(query
        );
        return TypedResults.Ok(url);
    }
}
