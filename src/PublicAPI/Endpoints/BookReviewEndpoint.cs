using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.BoundContext.BookReviewContext.Command.BookReview;
using Application.BoundContext.BookReviewContext.Command.Comment;
using Application.BoundContext.BookReviewContext.Command.Rating;
using Application.BoundContext.BookReviewContext.Queries;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class BookReviewEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("book-review");
        apis.MapPost("create", BookReviewEndpointService.CreateBookReview)
            .WithTags("BookReview")
            .WithName("CreateBookReview")
            .WithDescription("Creates a new book review.");
        apis.MapPost("comment/create", BookReviewEndpointService.CreateComment)
            .WithTags("Comment")
            .WithName("CreateComment")
            .WithDescription("Create comment");
        apis.MapPost("comment/update", BookReviewEndpointService.UpdateComment)
            .WithTags("Comment")
            .WithName("UpdateComment")
            .WithDescription("Update comment");
        apis.MapGet("comment/pagination/book", BookReviewEndpointService.GetCommentViewModelWithPaginationByBook)
            .WithTags("Comment")
            .WithName("GetCommentWithPaginationForBook")
            .WithDescription("Get comment with pagination for book");
        apis.MapGet("comment/pagination/user", BookReviewEndpointService.GetCommentViewModelWithPaginationByUserId)
            .WithTags("Comment")
            .WithName("GetCommentWithPaginationForUser")
            .WithDescription("Get comment with pagination for user");
        apis.MapGet("comment/all/book-and-user", BookReviewEndpointService.GetCommentWithUserAndBookId)
            .WithTags("GetCommentWithUserAndBookId")
            .WithName("GetCommentWithUserAndBookId")
            .WithDescription("Get comment with pagination for user");
        apis.MapPost("rating/create", BookReviewEndpointService.CreateRating)
            .WithTags("Rating")
            .WithName("CreateRating")
            .WithDescription("Create rating");
        apis.MapPost("rating/update", BookReviewEndpointService.UpdateRating)
            .WithTags("Rating")
            .WithName("UpdateRating")
            .WithDescription("Update rating");
        apis.MapGet("rating/pagination/book", BookReviewEndpointService.GetRatingViewModelWithPaginationByBook)
            .WithTags("Rating")
            .WithName("GetRatingWithPaginationForBook")
            .WithDescription("Get rating with pagination for book");
        apis.MapGet("rating/pagination/user", BookReviewEndpointService.GetRatingViewModelWithPaginationByUserId)
            .WithTags("Rating")
            .WithName("GetRatingWithPaginationForUser")
            .WithDescription("Get rating with pagination for user");
        apis.MapGet("rating/book-and-user", BookReviewEndpointService.GetRatingWithUserAndBookId)
            .WithName("Rating")
            .WithName("GetRatingWithBookAndUser")
            .WithDescription("Get rating with book and user");

    }
}


public static class BookReviewEndpointService
{
    [Authorize]
    public static async Task<Results<Ok<BookReviewViewModel>, BadRequest, ProblemHttpResult>> 
        CreateBookReview([FromBody] CreateBookReviewCommand command, 
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateBookReviewCommand, BookReviewViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<CommentViewModel>, BadRequest, ProblemHttpResult>> 
        CreateComment([FromBody] CreateCommentCommand command, 
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateCommentCommand, CommentViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<CommentViewModel>, BadRequest, ProblemHttpResult>> 
        UpdateComment([FromBody] UpdateCommentCommand command, 
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<UpdateCommentCommand, CommentViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<RatingViewModel>, BadRequest, ProblemHttpResult>> 
        CreateRating([FromBody] CreateRatingCommand command, 
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateRatingCommand, RatingViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<RatingViewModel>, BadRequest, ProblemHttpResult>> 
        UpdateRating([FromBody] UpdateRatingCommand command, 
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<UpdateRatingCommand, RatingViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<RatingViewModel>>, BadRequest, ProblemHttpResult>> 
        GetRatingViewModelWithPaginationByBook([FromQuery] Guid bookId,
            [AsParameters] PaginationRequest page,
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.BookReviewQueries.GetRatingWithPaginationByBookIdAsync(bookId, page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<CommentViewModel>>, BadRequest, ProblemHttpResult>> 
        GetCommentViewModelWithPaginationByBook([FromQuery] Guid bookId,
            [AsParameters] PaginationRequest page,
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.BookReviewQueries.GetCommentWithPaginationByBookIdAsync(bookId, page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<CommentViewModel>>, BadRequest, ProblemHttpResult>> 
        GetCommentViewModelWithPaginationByUserId(
            [AsParameters] PaginationRequest page,
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.BookReviewQueries.GetCommentWithPaginationByUserIdAsync(service.IdentityProvider.UserIdentity(), page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<RatingViewModel>>, BadRequest, ProblemHttpResult>> 
        GetRatingViewModelWithPaginationByUserId(
            [AsParameters] PaginationRequest page,
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.BookReviewQueries.GetRatingWithPaginationByUserIdAsync(service.IdentityProvider.UserIdentity(), page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<RatingViewModel>, BadRequest, ProblemHttpResult>> 
        GetRatingWithUserAndBookId(
            [FromQuery] Guid bookId,
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.BookReviewQueries.GetRatingByUSerIdAndBookIdAsync(service.IdentityProvider.UserIdentity(), bookId);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<CommentViewModel>>, BadRequest, ProblemHttpResult>> 
        GetCommentWithUserAndBookId(
            [FromQuery] Guid bookId,
            [FromServices] BookReviewServiceWrapper service)
    {
        var result = await service.BookReviewQueries.GetAllCommentByUserIdAndBookIdAsync(service.IdentityProvider.UserIdentity(), bookId);
        return TypedResults.Ok(result);
    }
}

public class BookReviewServiceWrapper(IFactoryHandler factoryHandler,
    ILogger<BookReviewServiceWrapper> logger,
    IIdentityProvider identityProvider,
    IBookReviewQueries bookReviewQueries)
{
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IBookReviewQueries BookReviewQueries { get; } = bookReviewQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookReviewServiceWrapper> Logger { get;} = logger;
}
