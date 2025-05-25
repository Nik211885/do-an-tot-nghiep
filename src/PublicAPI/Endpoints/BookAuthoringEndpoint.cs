using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.Command.Chapter;
using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.Query.Chapter;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;
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
        
        // endpoint for book
        apis.MapPost("book/create", BookAuthoringService.CreateBook)
            .WithName("CreateNewBook")
            .WithTags("Book")
            .WithDescription("Create New book");
        
        // endpoint for genre
        apis.MapPost("genre/create",  BookAuthoringService.CreateGenre)
            .WithTags("Genres")
            .WithName("CreateNewGenres")
            .WithDescription("Create New Genres");
        apis.MapPut("genre/update",  BookAuthoringService.UpdateGenre)
            .WithTags("Genres")
            .WithName("UpdateNewGenres")
            .WithDescription("Update New Genres");
        apis.MapPut("genre/change-active", BookAuthoringService.ChangeActiveGenre)
            .WithTags("Genres")
            .WithName("ChangeActiveGenres")
            .WithDescription("Change Active Genre");
        
        // endpoint for chapter
        apis.MapPost("chapter/create", BookAuthoringService.CreateChapter)
            .WithName("Chapter")
            .WithName("CreateNewChapter")
            .WithDescription("Create New Chapter");
        apis.MapPut("chapter/update", BookAuthoringService.UpdateChapter)
            .WithTags("Chapters")
            .WithName("UpdateChapter")
            .WithDescription("Update New Chapter");
        apis.MapGet("chapter/roll-back",  BookAuthoringService.RollBackChapter)
            .WithTags("Chapters")
            .WithName("RollBackChapter")
            .WithDescription("Rollback Chapter");
        apis.MapGet("chapter/preview-change-content", BookAuthoringService.PreviewChangeChapter)
            .WithTags("Chapters")
            .WithName("PreviewChangeChapter")
            .WithDescription("Preview Change Chapter");
        apis.MapPut("chapter/rename-version", BookAuthoringService.RenameChapterVersion)
            .WithTags("Chapters")
            .WithName("RenameChapterVersion")
            .WithDescription("Rename Chapter Version");
        apis.MapPost("chapter/submit-review", BookAuthoringService.SubmitAndReview)
            .WithName("SubmitReview")
            .WithTags("Chapters")
            .WithDescription("Submit Review");
    }
}

public static class BookAuthoringService
{
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>> 
        CreateBook([FromBody] CreateBookAuthoringCommand command, 
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateBookAuthoringCommand, BookViewModel>(command);
        return TypedResults.Ok(result);
    }
    
    [Authorize]
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>>
        CreateGenre([FromBody] CreateGenreCommand command,
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
    [Authorize]
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        ChangeActiveGenre([FromQuery] Guid id,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<ChangeActiveGenreCommand, GenreViewModel>(
                new ChangeActiveGenreCommand(Id:id));
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>>
        CreateChapter([FromBody] CreateChapterCommand command,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateChapterCommand, ChapterViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        UpdateChapter([FromQuery] Guid id, 
            [FromBody] UpdateChapterRequest request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<UpdateChapterCommand, ChapterViewModel>(
                new UpdateChapterCommand(Id:id, Request: request));
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
       RollBackChapter([AsParameters] RollBackChapterCommand request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<RollBackChapterCommand, ChapterViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterDiffContentViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        PreviewChangeChapter([AsParameters] GetPreviewChangeContentQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetPreviewChangeContentQuery, ChapterDiffContentViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        RenameChapterVersion([AsParameters]  RenameChapterVersionCommand request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<RenameChapterVersionCommand, ChapterViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        SubmitAndReview([AsParameters] SubmitAndReviewChapterCommand request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<SubmitAndReviewChapterCommand, ChapterViewModel>(request);
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
