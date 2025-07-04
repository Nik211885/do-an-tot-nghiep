using Application.BoundContext.BookReviewContext.Queries;
using Application.BoundContext.UserProfileContext.Command.Favorite;
using Application.BoundContext.UserProfileContext.Command.Follower;
using Application.BoundContext.UserProfileContext.Command.SearchHistory;
using Application.BoundContext.UserProfileContext.Command.UserProfile;
using Application.BoundContext.UserProfileContext.Queries;
using Application.BoundContext.UserProfileContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.IdentityProvider;
using Application.Models;
using Application.Models.KeyCloak;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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
        apis.MapPut("update", UserProfileEndpointService.UpdateUserProfile)
            .WithTags("UserProfile")
            .WithName("UpdateUserProfile")
            .WithDescription("Updated user profile");
        apis.MapGet("by-ids", UserProfileEndpointService.GetUserInfoByIds)
            .WithTags("UserProfile")
            .WithName("GetUserProfileByIds")
            .WithDescription("Get user profile by ids");
        apis.MapPut("reset-password-by-email", UserProfileEndpointService.ResetPasswordByEmail)
            .WithTags("UserProfile")
            .WithName("ResetPasswordByEmail")
            .WithDescription("Reset password by email");
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
        apis.MapGet("book/my-favorite/pagination", UserProfileEndpointService.GetFavoriteWithPaginationByUserId)
            .WithTags("Favorite")
            .WithName("FavoriteWithPaginationByUserId")
            .WithDescription("Favorite book with pagination for user id");
        apis.MapGet("book/my-favorite/pagination/v1", UserProfileEndpointService.GetFavoriteAggregateWithPaginationByUserId)
            .WithTags("Favorite")
            .WithName("FavoriteWithPaginationByUserIdV1")
            .WithDescription("Favorite book with pagination for user idV1");
        apis.MapPost("book/favorite-in", UserProfileEndpointService.GetFavoriteWithBookInForUser)
            .WithTags("Favorite")
            .WithName("GetFavoriteInCollectionBookHasSpecific")
            .WithDescription("Get favorite in collection book has specific");
        apis.MapPost("search-history/create", UserProfileEndpointService.CreateSearchHistory)
            .WithTags("SearchHistory")
            .WithName("CreateSearchHistory")
            .WithDescription("Create new search history");
        apis.MapDelete("search-history/delete", UserProfileEndpointService.DeleteSearchHistory)
            .WithTags("SearchHistory")
            .WithName("DeleteSearchHistory")
            .WithDescription("Delete search history");
        apis.MapDelete("search-history/clean", UserProfileEndpointService.CleanMySearchHistory)
            .WithTags("SearchHistory")
            .WithName("CleanMySearchHistory")
            .WithDescription("Delete search history");
        apis.MapGet("search-history/pagination", UserProfileEndpointService.GetSearchHistoryWithPagination)
            .WithTags("SearchHistory")
            .WithName("GetSearchHistoryWithPagination")
            .WithDescription("Get search history with pagination");
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
            [FromBody] UpdateUserRequest userUpdate,
            [FromServices] UserProfileServiceWrapper service,
            [FromServices] IIdentityProviderServices identityProviderServices
        )
    {
        var command = new UpdateUserProfileCommand(service.IdentityProvider.UserIdentity(), userUpdate);
        var result = await service.FactoryHandler.Handler<UpdateUserProfileCommand, UserProfileViewModel>(command);
        var updatedFromKeyCloakService = await identityProviderServices.UpdateUserInfoAsync(command);
        return updatedFromKeyCloakService
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest();
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
    [Authorize]
    public static async Task<Results<Ok<SearchHistoryViewModel>, BadRequest, ProblemHttpResult>> 
        CreateSearchHistory(
            [FromBody] CreateSearchHistoryCommand  command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.FactoryHandler
            .Handler<CreateSearchHistoryCommand, SearchHistoryViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<NoContent, BadRequest, ProblemHttpResult>> 
        DeleteSearchHistory(
            [FromBody] DeleteSearchHistoryCommand  command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        await service.FactoryHandler
            .Handler<DeleteSearchHistoryCommand, bool>(command);
        return TypedResults.NoContent();
    }
    [Authorize]
    public static async Task<Results<NoContent, BadRequest, ProblemHttpResult>> 
        CleanMySearchHistory(
            [AsParameters] CleanAllSearchHistoryCommand  command,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        await service.FactoryHandler
            .Handler<CleanAllSearchHistoryCommand, bool>(command);
        return TypedResults.NoContent();
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<SearchHistoryViewModel>>, BadRequest, ProblemHttpResult>> 
        GetSearchHistoryWithPagination(
            [AsParameters] PaginationRequest page,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.UserProfileQueries
            .GetSearchHistoryWithPaginationByUserIdAsync(service.IdentityProvider.UserIdentity(), page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<FavoriteBookViewModel>>, BadRequest, ProblemHttpResult>> 
        GetFavoriteWithBookInForUser(
            [FromBody] Guid[] ids,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.UserProfileQueries
            .GetFavoriteWithBookInAndForUserIdAsync(
                service.IdentityProvider.UserIdentity(),
                bookIds: ids
            );
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<FavoriteBookViewModel>>, BadRequest, ProblemHttpResult>> 
        GetFavoriteWithPaginationByUserId(
            [AsParameters] PaginationRequest page,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.UserProfileQueries
            .GetFavoriteBookWithPaginationByUserIdAsync(
                service.IdentityProvider.UserIdentity(),
                page
            );
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<FavoriteBookViewModelAggregate>>, BadRequest, ProblemHttpResult>> 
        GetFavoriteAggregateWithPaginationByUserId(
            [AsParameters] PaginationRequest page,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var favoriteBook = await service.UserProfileQueries
            .GetFavoriteBookWithPaginationByUserIdAsync(
                service.IdentityProvider.UserIdentity(),
                page
            );
        var ids = favoriteBook.Items.Select(x => x.FavoriteBookId).ToList();
        var review = await service.BookReviewQueries
            .GetBookReviewByBookIdsAsync(CancellationToken.None, ids.ToArray());
        var book = await service.BookServices
            .ListAsync(new QueryParamRequest(),
                filter: q => q.Bool(b => b
                    .Must(BookPublicEndpointServices.BookActive,
                        m=>m.Terms(t=>t.Field("id.keyword").Terms(new TermsQueryField(
                            ids.Select(x=>FieldValue.String(x.ToString())).ToArray()
                        )))
                    )));
        var aggregate = favoriteBook.ToPaginationAggregate(book.Documents,
            review);
        return TypedResults.Ok(aggregate);
    }
    
    [Authorize]
    public static async Task<Results<NoContent, BadRequest, ProblemHttpResult>> 
        ResetPasswordByEmail(
            [FromQuery] string clientId,
            [FromQuery] string returnUri,
            [FromServices] UserProfileServiceWrapper service
        )
    {
        var result = await service.IdentityProviderServices
            .ResetPasswordAsync(service.IdentityProvider
                .UserIdentity(), clientId, returnUri);
        return result ?
            TypedResults.NoContent() :
            TypedResults.BadRequest();
    }
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<UserInfo>>, BadRequest, ProblemHttpResult>>
        GetUserInfoByIds(string[] userIds, [FromServices] UserProfileServiceWrapper service)
    {
        var tasks = userIds.Distinct().Select(service.IdentityProviderServices.GetUserInfoAsync);
        var result = await Task.WhenAll(tasks);
        var nonNullResults = result
            .Where(user => user is not null)
            .Cast<UserInfo>()
            .ToList();
        return TypedResults.Ok((IReadOnlyCollection<UserInfo>)nonNullResults);
    }
}

public class UserProfileServiceWrapper(
    IFactoryHandler factoryHandler,
    IElasticServices<BookElasticModel> bookServices,
    IBookReviewQueries bookReviewsQueries,
    ILogger<UserProfileServiceWrapper> logger,
    IIdentityProvider identityProvider,
    IIdentityProviderServices identityProviderServices,
    IUserProfileQueries userProfileQueries)
{
    public IElasticServices<BookElasticModel> BookServices { get; } = bookServices;
    public IBookReviewQueries BookReviewQueries { get; } = bookReviewsQueries;
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IIdentityProviderServices IdentityProviderServices { get; } = identityProviderServices;
    public IFactoryHandler FactoryHandler {get;} = factoryHandler;
    public ILogger<UserProfileServiceWrapper> Logger {get;} = logger;
    public IUserProfileQueries UserProfileQueries {get;} = userProfileQueries;
}
