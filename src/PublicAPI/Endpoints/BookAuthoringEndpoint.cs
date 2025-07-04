using Application.BoundContext.BookAuthoringContext.Command.Book;
using Application.BoundContext.BookAuthoringContext.Command.Chapter;
using Application.BoundContext.BookAuthoringContext.Command.Genre;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.Query.Book;
using Application.BoundContext.BookAuthoringContext.Query.Chapter;
using Application.BoundContext.BookAuthoringContext.Query.Genre;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Models;
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
        apis.MapPost("book/update", BookAuthoringService.UpdateBook)
            .WithName("UpdateNewBook")
            .WithTags("Book")
            .WithDescription("Update New book");
        apis.MapPut("book/change-release-type", BookAuthoringService.ChangeReleaseTypeBook)
            .WithTags("Book")
            .WithName("ChangeReleaseType")
            .WithDescription("Change release type");
        apis.MapPut("book/change-policy", BookAuthoringService.ChangeBookPolicy)
            .WithTags("Book")
            .WithName("ChangePolicy")
            .WithDescription("Change policy");
        apis.MapGet("book/my-book", BookAuthoringService.GetAllMyBooks)
            .WithTags("Book")
            .WithName("GetMyBook")
            .WithDescription("Get My books");
        apis.MapGet("book/slug", BookAuthoringService.GetBookDetailBySlug)
            .WithTags("Book")
            .WithName("GetBookBySlug")
            .WithDescription("Get book by Slug");
        apis.MapDelete("book/delete", BookAuthoringService.DeleteBook)
            .WithTags("Book")
            .WithName("DeleteBook")
            .WithDescription("Delete book");
        apis.MapGet("book/id", BookAuthoringService.GetBookById)
            .WithTags("Book")
            .WithName("GetBookById")
            .WithDescription("Get Book By Id");
        apis.MapPut("book/mark-complete", BookAuthoringService.MarkBookComplete)
            .WithTags("Book")
            .WithName("MarkBookComplete")
            .WithDescription("Mark book complete");
        apis.MapGet("book/pagination/filter", BookAuthoringService.FindBookWithFilter)
            .WithTags("Book")
            .WithName("FindBookWithPaginationForFilter")
            .WithDescription("Find books with pagination for filter");
        
        
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
        apis.MapGet("genre/all", BookAuthoringService.GetAllGenres)
            .WithTags("Genres")
            .WithName("GetAllGenres")
            .WithDescription("Get all Genres");
        apis.MapGet("genre/slug", BookAuthoringService.GetGenreBySlug)
            .WithTags("Genres")
            .WithName("GetGenreBySlug")
            .WithDescription("Get Genre by Slug");
        apis.MapGet("top/genre", BookAuthoringService
            .GetTopGenreHasMaxBookChoose)
            .WithTags("Genres")
            .WithName("GetTopGenresHasMaxBookChoose")
            .WithDescription("Get top genres has max book choose");
        apis.MapGet("genre/pagination", BookAuthoringService
            .GetGenreWithPagination)
            .WithTags("Genres")
            .WithName("GetGenreWithPagination")
            .WithDescription("Get Genre With Pagination");
        apis.MapGet("genre/id", BookAuthoringService.GetGenreById)
            .WithTags("Genres")
            .WithName("GetGenreById")
            .WithDescription("Get genere by id");
            
        
        // endpoint for chapter
        apis.MapPost("chapter/create", BookAuthoringService.CreateChapter)
            .WithTags("Chapter")
            .WithName("CreateNewChapter")
            .WithDescription("Create New Chapter");
        apis.MapPut("chapter/update", BookAuthoringService.UpdateChapter)
            .WithTags("Chapter")
            .WithName("UpdateChapter")
            .WithDescription("Update New Chapter");
        apis.MapGet("chapter/roll-back",  BookAuthoringService.RollBackChapter)
            .WithTags("Chapter")
            .WithName("RollBackChapter")
            .WithDescription("Rollback Chapter");
        apis.MapGet("chapter/preview-change-content", BookAuthoringService.PreviewChangeChapter)
            .WithTags("Chapter")
            .WithName("PreviewChangeChapter")
            .WithDescription("Preview Change Chapter");
        apis.MapPut("chapter/rename-version", BookAuthoringService.RenameChapterVersion)
            .WithTags("Chapter")
            .WithName("RenameChapterVersion")
            .WithDescription("Rename Chapter Version");
        apis.MapPost("chapter/submit-review", BookAuthoringService.SubmitAndReview)
            .WithName("SubmitReview")
            .WithTags("Chapter")
            .WithDescription("Submit Review");
        apis.MapGet("chapter/by-book-slug", BookAuthoringService.GetAllChapterByBookSlug)
            .WithTags("Chapter")   
            .WithName("GetChapterByBookSlug")
            .WithDescription("Get Chapter by Book Slug");
        apis.MapGet("chapter/chapter-version",BookAuthoringService.GetAllChapterVersionByChapterId)
            .WithTags("Chapter")
            .WithName("GetChapterVersionByChapterId")
            .WithDescription("Get Chapter Version by Chapter Id");
        apis.MapGet("chapter/slug", BookAuthoringService.GetChapterBySlug)
            .WithTags("Chapter")
            .WithName("GetChapterBySlug")
            .WithDescription("Get Chapter by Slug");
        apis.MapDelete("chapter/delete", BookAuthoringService.DeleteChapter)
            .WithTags("Chapter")
            .WithName("DeleteChapter")
            .WithDescription("Delete Chapter");

    }
}

public static class BookAuthoringService
{
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>> 
        CreateBook([FromBody] CreateBookCommand command, 
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<CreateBookCommand, BookViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, ProblemHttpResult>> 
        UpdateBook([FromQuery] Guid id,
            [FromBody] UpdateBookRequest request, 
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var command = new UpdateBookCommand(id, request);
        var result = await service.FactoryHandler
            .Handler<UpdateBookCommand, BookViewModel>(command);
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
    public static async Task<Results<NoContent, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        RenameChapterVersion([AsParameters]  RenameChapterVersionCommand request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        _ = await service.FactoryHandler
            .Handler<RenameChapterVersionCommand, string>(request);
        return TypedResults.NoContent();
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
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        ChangeReleaseTypeBook([AsParameters] ChangeBookReleaseTypeCommand request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<ChangeBookReleaseTypeCommand, BookViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        ChangeBookPolicy([AsParameters] ChangePolicyBookCommand request,
            [FromServices] BookAuthoringServiceWrapper service) 
    {
        var result = await service.FactoryHandler
            .Handler<ChangePolicyBookCommand, BookViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<BookViewModel>>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetAllMyBooks([FromServices] IIdentityProvider identityProvider,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var request = new GetAllBooksByUserIdQuery(identityProvider.UserIdentity());
        var result = await service.FactoryHandler
            .Handler<GetAllBooksByUserIdQuery, IReadOnlyCollection<BookViewModel>>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<BookViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetBookDetailBySlug([AsParameters] GetBookDetailBySlugQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetBookDetailBySlugQuery, BookViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<ChapterViewModel>>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetAllChapterByBookSlug([AsParameters] GetAllChapterByBookSlugQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetAllChapterByBookSlugQuery, IReadOnlyCollection<ChapterViewModel>>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetAllChapterVersionByChapterId([AsParameters] GetAllChapterVersionByChapterIdQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetAllChapterVersionByChapterIdQuery, ChapterViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetChapterBySlug([AsParameters] GetChapterBySlugQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetChapterBySlugQuery, ChapterViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<ChapterDiffContentViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetPreviewChangeContent([AsParameters] GetPreviewChangeContentQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetPreviewChangeContentQuery, ChapterDiffContentViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<IReadOnlyCollection<GenreViewModel>>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetAllGenres([AsParameters] GetAllGenresQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetAllGenresQuery, IReadOnlyCollection<GenreViewModel>>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetGenreBySlug([AsParameters] GetGenreBySlugQuery request,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler
            .Handler<GetGenreBySlugQuery,GenreViewModel>(request);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<NoContent, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        DeleteChapter([AsParameters] DeleteChapterCommand command,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        await service.FactoryHandler.Handler<DeleteChapterCommand, string>(command);
        return TypedResults.NoContent();    
    }
    [Authorize]
    public static async Task<Results<NoContent, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        DeleteBook([AsParameters] DeleteBookCommand command,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        await service.FactoryHandler.Handler<DeleteBookCommand, string>(command);
        return TypedResults.NoContent();    
    }

    [Authorize]
    public static async Task<Ok<BookViewModel>> GetBookById([AsParameters] GetBookDetailByIdQuery query,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler.Handler<GetBookDetailByIdQuery, BookViewModel?>(query);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Ok<BookViewModel>> MarkBookComplete([AsParameters] MarkCompletedBookCommand command,
        [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.FactoryHandler.Handler<MarkCompletedBookCommand, BookViewModel>(command);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<BookViewModel>>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        FindBookWithFilter(
            [AsParameters] PaginationRequest page,
            [AsParameters] BookAuthoringQueriesRequest.FilterBookAuthoring filter,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.BookAuthoringQueries.FindBookWithPaginationForUserIdAsync(
            service.IdentityProvider.UserIdentity(), filter, page);
        return TypedResults.Ok(result);
    }
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetTopGenreHasMaxBookChoose(
            [FromQuery] int top,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.BookAuthoringQueries
            .GetTopGenresHasManyBookAsync(top);
        return TypedResults.Ok(result);
    }
    public static async Task<Results<Ok<PaginationItem<GenreViewModel>>, UnauthorizedHttpResult, BadRequest, NotFound, ProblemHttpResult>>
        GetGenreWithPagination(
            [AsParameters] PaginationRequest page,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.BookAuthoringQueries
            .FindGenreWithPagination(CancellationToken.None, page);
        return TypedResults.Ok(result);
    }
    public static async Task<Results<Ok<GenreViewModel>, UnauthorizedHttpResult, NotFound, ProblemHttpResult>>
        GetGenreById(
            [FromQuery] Guid id,
            [FromServices] BookAuthoringServiceWrapper service)
    {
        var result = await service.BookAuthoringQueries
            .FindGenreByIdAsync(id);
        return result is null ? TypedResults.NotFound()
                : TypedResults.Ok(result);
    }
    
}

public class BookAuthoringServiceWrapper(IFactoryHandler factoryHandler,
    ILogger<BookAuthoringServiceWrapper> logger,
    IIdentityProvider identityProvider,
    IBookAuthoringQueries bookAuthoringQueries)
{
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IBookAuthoringQueries BookAuthoringQueries { get; } = bookAuthoringQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookAuthoringServiceWrapper> Logger { get;} = logger;
}
