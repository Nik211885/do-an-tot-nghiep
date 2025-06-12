using Application.BoundContext.NotificationContext.Command;
using Application.BoundContext.NotificationContext.ViewModel;
using Application.BoundContext.UserProfileContext.Command.Favorite;
using Application.BoundContext.UserProfileContext.Command.Follower;
using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.BoundContext.UserProfileContext.Queries;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
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
        apis.MapGet("update", UserProfileEndpointService.UpdateUserProfile)
            .WithTags("UserProfile")
            .WithName("UpdateUserProfile")
            .WithDescription("Updated user profile");
        apis.MapGet("by",  UserProfileEndpointService.GetUserProfileById)
            .WithTags("UserProfile")
            .WithName("GetUserProfileById")
            .WithDescription("Get user profile by id");
        apis.MapGet("my", UserProfileEndpointService.GetMyProfile)
            .WithTags("UserProfile")
            .WithName("GetMyProfile")
            .WithDescription("Get user profile");
        apis.MapGet("following", UserProfileEndpointService.Following)
            .WithTags("Following")
            .WithName("CreateFollowing")
            .WithDescription("Create following user");
        apis.MapGet("un-following", UserProfileEndpointService.UnFollowing)
            .WithTags("Following")
            .WithName("UnFollowing")
            .WithDescription("Un following user");
        apis.MapGet("book/favorite", UserProfileEndpointService.FavoriteBook)
            .WithTags("Favorite")
            .WithName("FavoriteBook")
            .WithDescription("Favorite book");
        apis.MapGet("book/un-favorite", UserProfileEndpointService.UnFavoriteBook)
            .WithTags("Favorite")
            .WithName("UnFavoriteBook")
            .WithDescription("UnFavorite book");
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
    [Authorize]
    public static async Task<Results<Ok<UserProfileViewModel>, BadRequest, ProblemHttpResult>> 
        UpdateUserProfile(
            [FromBody] string bio,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var command = new UpdateUserProfileCommand(service.IdentityProvider.UserIdentity(), bio);
        var result = await service.FactoryHandler.Handler<UpdateUserProfileCommand, UserProfileViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<UserProfileViewModel>, BadRequest, ProblemHttpResult>> 
        GetUserProfileById(
            [FromQuery] Guid id,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.UserProfileQueries.GetUserProfileByIdAsync(id);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<UserProfileViewModel>, BadRequest, ProblemHttpResult>> 
        GetMyProfile(
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.UserProfileQueries.GetUserProfileByIdAsync(service.IdentityProvider
            .UserIdentity());
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<FollowerViewModel>, BadRequest, ProblemHttpResult>> 
        Following(
            [AsParameters] CreateFollowerCommand command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.FactoryHandler
            .Handler<CreateFollowerCommand, FollowerViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<FollowerViewModel>, BadRequest, ProblemHttpResult>> 
        UnFollowing(
            [AsParameters] DeleteFollowerCommand  command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.FactoryHandler
            .Handler<DeleteFollowerCommand, FollowerViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<FavoriteBookViewModel>, BadRequest, ProblemHttpResult>> 
        FavoriteBook(
            [AsParameters] CreateFavoriteBookCommand  command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.FactoryHandler
            .Handler<CreateFavoriteBookCommand, FavoriteBookViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<FavoriteBookViewModel>, BadRequest, ProblemHttpResult>> 
        UnFavoriteBook(
            [AsParameters] UnFavoriteBookCommand  command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.FactoryHandler
            .Handler<UnFavoriteBookCommand, FavoriteBookViewModel>(command);
        return TypedResults.Ok(result);
    }
}

public class UserProfileServiceWrapper(
    IFactoryHandler factoryHandler,
    ILogger<UserProfileServiceWrapper> logger,
    IIdentityProvider identityProvider,
    IUserProfileQueries userProfileQueries)
{
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IFactoryHandler FactoryHandler {get;} = factoryHandler;
    public ILogger<UserProfileServiceWrapper> Logger {get;} = logger;
    public IUserProfileQueries UserProfileQueries {get;} = userProfileQueries;
}
