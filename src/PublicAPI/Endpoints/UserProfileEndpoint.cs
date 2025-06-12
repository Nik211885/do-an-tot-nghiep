using Application.BoundContext.NotificationContext.Command;
using Application.BoundContext.NotificationContext.ViewModel;
using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.BoundContext.UserProfileContext.Queries;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class UserProfileEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("user-profile");
        apis.MapPost("create", UserProfileEndpointService.CreateUserProfile)
            .WithTags("UserProfile")
            .WithName("CreateUserProfile")
            .WithDescription("Creat new user profile");
    }
}

public static class UserProfileEndpointService
{
    [Authorize]
    public static async Task<Results<Ok<UserProfileViewModel>, BadRequest, ProblemHttpResult>> 
        CreateUserProfile(
        [FromBody] CreateUserProfileCommand command,
        [FromServices] UserProfileServiceWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<CreateUserProfileCommand, UserProfileViewModel>(command);
        return TypedResults.Ok(result);
    }
}

public class UserProfileServiceWrapper(
    IFactoryHandler factoryHandler,
    ILogger<UserProfileServiceWrapper> logger,
    IUserProfileQueries userProfileQueries)
{
    public IFactoryHandler FactoryHandler {get;} = factoryHandler;
    public ILogger<UserProfileServiceWrapper> Logger {get;} = logger;
    public IUserProfileQueries UserProfileQueries {get;} = userProfileQueries;
}
