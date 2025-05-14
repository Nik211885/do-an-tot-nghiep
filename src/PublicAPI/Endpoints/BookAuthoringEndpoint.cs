using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.Query;
using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.GenresAggregate;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class BookAuthoringEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("book-authoring");
        apis.MapGet("genre", BookAuthoringService.GetAllGenreActive)
            .WithName("ListGenreActive")
            .WithTags("Genres")
            .WithDescription("Get all genre active");
    }
}

public static class BookAuthoringService
{
    public static async Task<Results<Ok<IReadOnlyCollection<Genres>>, ProblemHttpResult>> GetAllGenreActive(
        [FromServices] BookAuthoringServiceWrapper service
        )
    {
        var query = new GetAllGenreActiveQuery();
        var result = await service.FactoryHandler.Handler<GetAllGenreActiveQuery,IReadOnlyCollection<Genres>>(query);
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
