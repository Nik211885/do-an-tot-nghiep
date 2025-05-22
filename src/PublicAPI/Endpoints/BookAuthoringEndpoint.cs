using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.Query;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class BookAuthoringEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("book-authoring");
        apis.MapPost("book/create", BookAuthoringService.CreateNewBook)
            .WithName("CreateNewBook")
            .WithTags("Book")
            .WithDescription("Create New book");
        apis.MapGet("genre", BookAuthoringService.GetAllGenreActive)
            .WithTags("Genres")
            .WithName("ListGenreActive")
            .WithDescription("Get all genre active");
        apis.MapPost("genre/create",  BookAuthoringService.CreateNewGenre)
            .WithTags("Genres")
            .WithName("CreateNewGenres")
            .WithDescription("Create New Genres");
        apis.MapPost("genre/update",  BookAuthoringService.UpdateGenre)
            .WithTags("Genres")
            .WithName("UpdateNewGenres")
            .WithDescription("Update New Genres");
    }
}

public static class BookAuthoringService
{
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<Genres>>, ProblemHttpResult>> GetAllGenreActive(
        [FromServices] BookAuthoringServiceWrapper service
        )
    {
        var query = new GetAllGenreActiveQuery();
        var result = await service.FactoryHandler.Handler<GetAllGenreActiveQuery,IReadOnlyCollection<Genres>>(query);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>> 
        CreateNewBook([FromBody] CreateBookAuthoringCommand command, 
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateBookAuthoringCommand, BookViewModel>(command);
        return TypedResults.Ok(result);
    }
    
    [Authorize]
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>>
        CreateNewGenre([FromBody] CreateGenreCommand command,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler.Handler<CreateGenreCommand, GenreViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        UpdateGenre([FromQuery] Guid id, [FromBody] UpdateGenreRequest request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<UpdateGenreCommand, GenreViewModel>(
                new UpdateGenreCommand(Id:id, Request:request));
        return TypedResults.Ok(result);
    }
}

public class BookAuthoringServiceWrapper(IFactoryHandler factoryHandler,
    ILogger<BookAuthoringServiceWrapper> logger,
    IBookAuthoringQueries bookAuthoringQueries)
{
    public IBookAuthoringQueries BookAuthoringQueries { get; } = bookAuthoringQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookAuthoringServiceWrapper> Logger { get;} = logger;
}
