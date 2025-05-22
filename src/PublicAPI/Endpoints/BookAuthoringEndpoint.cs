using Application.BoundContext.BookAuthoringContext.Command;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.Query;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
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

        apis.MapPost("create-book", BookAuthoringService.CreateNewBook)
            .WithName("CreateNewBook")
            .WithDescription("Create New book");
        apis.MapGet("genre", BookAuthoringService.GetAllGenreActive)
            .WithName("ListGenreActive")
            .WithDescription("Get all genre active");
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
    public static async Task<Results<Ok<Book>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>> 
        CreateNewBook([FromBody] CreateBookAuthoringCommand authoringCommand, 
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler.Handler<CreateBookAuthoringCommand, Book>(authoringCommand);
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
