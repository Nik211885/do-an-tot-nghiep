using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.BoundContext.BookReviewContext.Command.BookReview;
using Application.BoundContext.BookReviewContext.Command.Comment;
using Application.BoundContext.BookReviewContext.Command.Rating;
using Application.BoundContext.BookReviewContext.Queries;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Interfaces.CQRS;
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
        apis.MapPost("rating/create", BookReviewEndpointService.CreateRating)
            .WithTags("Rating")
            .WithName("CreateRating")
            .WithDescription("Create rating");
        apis.MapPost("rating/update", BookReviewEndpointService.UpdateRating)
            .WithTags("Rating")
            .WithName("UpdateRating")
            .WithDescription("Update rating");

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
}

public class BookReviewServiceWrapper(IFactoryHandler factoryHandler,
    ILogger<BookReviewServiceWrapper> logger,
    IBookReviewQueries bookReviewQueries)
{
    public IBookReviewQueries BookReviewQueries { get; } = bookReviewQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookReviewServiceWrapper> Logger { get;} = logger;
}
