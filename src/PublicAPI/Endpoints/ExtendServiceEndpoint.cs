using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.UploadFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class ExtendServiceEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var api = endpoint.MapGroup("extend-service");
        api.MapGet("upload-file-by-singature", [Authorize](
            [FromServices] IUploadFileServices uploadFileService
        ) =>
        {
            var urlUploadFileBySignature = uploadFileService.GetUrlUploadFileBySignature();
            return TypedResults.Ok(urlUploadFileBySignature);
        })
        .WithName("UploadFileBySignature")
        .WithTags("ExtendService")
        .WithDescription("Extend service upload file by signature");
    }
}
