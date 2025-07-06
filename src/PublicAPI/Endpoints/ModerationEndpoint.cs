using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Common;
using Application.Common.Authorization;
using Application.Interfaces.CQRS;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class ModerationEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("moderation");
        apis.MapPost("create/chapter", ModerationEndpointServices.CreateModeration)
            .WithTags("ModerationChapter")
            .WithName("CreatModerationChapter")
            .WithDescription("Create moderation chapter");
        apis.MapPost("approval/chapter", ModerationEndpointServices.ApproveModeration)
            .WithTags("ModerationChapter")
            .WithName("ApproveModerationChapter")
            .WithDescription("Approve moderation chapter");
        apis.MapPost("reject/chapter", ModerationEndpointServices.RejectModeration)
            .WithTags("ModerationChapter")
            .WithName("RejectModerationChapter")
            .WithDescription("Reject moderation chapter");
        apis.MapGet("approval", ModerationEndpointServices.GetBookApprovalWithPaginationByStatusAsync)
            .WithTags("ModerationChapter")
            .WithName("GetBookApprovalWithPaginationByStatus")
            .WithDescription("Get book approval with pagination by status");
        apis.MapGet("approval/id", ModerationEndpointServices.GetBookApprovalById)
            .WithTags("ModerationChapter")
            .WithName("GetBookApprovalById")
            .WithDescription("Get book approval with ID");
        apis.MapGet("decision/pagination", ModerationEndpointServices.GetBookApprovalDecisionByApprovalId)
            .WithTags("ModerationChapter")
            .WithName("GetDecisionWithPaginationByApprovalId")
            .WithDescription("Get book approval with pagination ID");
        apis.MapPost("add-signature", ModerationEndpointServices.AddSignature)
            .WithTags("ModerationChapter")
            .WithName("AddSignature")
            .WithDescription("Add signature");
        apis.MapGet("chapter-approval", ModerationEndpointServices.GetChapterForBook)
            .WithTags("ModerationChapter")
            .WithName("GetChapterApprovalInModeration")
            .WithDescription("Get chapter approval in moderation");
        apis.MapGet("repository", ModerationEndpointServices.GetBookApprovalRepositoryGroupByBookId)
            .WithTags("ModerationChapter")
            .WithName("GetBookApprovalForRepositoryGroupByBookId")
            .WithDescription("Get book approval for repository group by book ID");
        apis.MapGet("approval-for-book", ModerationEndpointServices.GetBookApprovalForBookId)
            .WithTags("ModerationChapter")
            .WithName("GetBookApprovalForBookId")
            .WithDescription("Get book approval for book ID");
    }
}

public static class ModerationEndpointServices
{
    [Authorize]
    public static async Task<Results<Ok<BookApprovalViewModel>, ProblemHttpResult>> 
        CreateModeration(
        [FromBody] CreateBookApprovalCommand command,
        [FromServices] ModerationServiceWrapper service
        )
    {
        var result = await service.FactoryHandler.Handler<CreateBookApprovalCommand, BookApprovalViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Moderation)]
    public static async Task<Results<Ok<BookApprovalViewModel>, ProblemHttpResult>> 
        ApproveModeration(
        [FromBody] ApprovalBookCommand command,
        [FromServices] ModerationServiceWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<ApprovalBookCommand, BookApprovalViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Moderation)]
    public static async Task<Results<Ok<BookApprovalViewModel>, ProblemHttpResult>> 
        RejectModeration(
        [FromBody] RejectBookCommand command,
        [FromServices] ModerationServiceWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<RejectBookCommand, BookApprovalViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Moderation)]
    public static async Task<Results<Ok<PaginationItem<BookApprovalViewModel>>,
            ProblemHttpResult>>  GetBookApprovalWithPaginationByStatusAsync
    (
        [AsParameters] PaginationRequest page, [FromQuery] BookApprovalStatus status,
        [FromServices] ModerationServiceWrapper service
    )
    {
        var result =  await service.ModerationQueries.GetBookApprovalWithPaginationByStatusAsync(status, page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<Ok<BookApprovalViewModel>, ProblemHttpResult>>  
        AddSignature
    (
         [FromBody] CreateSignatureCommand command,
        [FromServices] ModerationServiceWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<CreateSignatureCommand, BookApprovalViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Moderation)]
    public static async Task<Results<Ok<BookApprovalViewModel>, ProblemHttpResult>>  
        GetBookApprovalById
    (
        [FromQuery] Guid bookApprovalId,
        [FromServices] ModerationServiceWrapper service
    )
    {
        var result =  await service.ModerationQueries
            .GetBookApprovalByIdAsync(bookApprovalId);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Moderation)]
    public static async Task<Results<Ok<PaginationItem<ApprovalDecisionViewModel>>, ProblemHttpResult>>  
        GetBookApprovalDecisionByApprovalId
        (
            [FromQuery] Guid bookApprovalId,
            [AsParameters] PaginationRequest page,
            [FromServices] ModerationServiceWrapper service
        )
    {
        var result =  await service.ModerationQueries
            .GetDecisionWithPaginationByApprovalIdAsync(bookApprovalId, page);
        return TypedResults.Ok(result);
    }

    public static async Task<Results<Ok<IReadOnlyCollection<ChapterStoreViewModel>>, NotFound>>
        GetChapterForBook(
            [FromQuery] Guid bookId,
            [FromServices] ModerationServiceWrapper service
        )
    {
        // Check book has exits if don't has exits just return 404
        // Or when user has buyer success just return 403
        var result = await service.ModerationQueries.GetAllChapterSuccessForBookIdAsync(bookId);
        return TypedResults.Ok(result);
    }
    public static async Task<Results<Ok<PaginationItem<ApprovalRepositoryViewModel>>, NotFound>>
        GetBookApprovalRepositoryGroupByBookId(
            [FromQuery]Guid? bookId,
            string? bookTitle,
            [AsParameters] PaginationRequest page,
            [FromServices] ModerationServiceWrapper service
        )
    {
        var result = await service
            .ModerationQueries.GetApprovalRepositoryGroupByBookIdAsync(bookId,bookTitle,page);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Moderation)]
    public static async Task<Results<Ok<IReadOnlyCollection<BookApprovalViewModel>>, NotFound>>
        GetBookApprovalForBookId(
            [FromQuery]Guid bookId,
            [FromServices] ModerationServiceWrapper service
        )
    {
        var result = await service
            .ModerationQueries.GetAllBookApprovalsByBookIdAsync(bookId);
        return TypedResults.Ok(result);
    }
}


public class ModerationServiceWrapper(IFactoryHandler factoryHandler,
    ILogger<BookAuthoringServiceWrapper> logger,
    IModerationQueries moderationQueries)
{
    public IModerationQueries ModerationQueries { get; } = moderationQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookAuthoringServiceWrapper> Logger { get;} = logger;
}
