using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Common;
using Application.Common.Authorization;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class ModerationEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("moderation");
        apis.MapPut("active", ModerationEndpointService.ActiveBook)
            .WithTags("Moderation")
            .WithName("ActiveBook")
            .WithDescription("Active for book approval");
        apis.MapPut("unactive", ModerationEndpointService.UnActiveBook)
            .WithTags("Moderation")
            .WithName("UnActiveBook")
            .WithDescription("Unactive for book approval");
        apis.MapPost("chapter/approval", ModerationEndpointService.ApprovalChapter)
            .WithTags("Moderation")
            .WithName("ChapterApproval")
            .WithDescription("Chapter approval");
        apis.MapPost("book/create", ModerationEndpointService.CreateBookApproval)
            .WithTags("Moderation")
            .WithName("CreateBookApproval")
            .WithDescription("Create book approval");
        apis.MapPost("chapter/create", ModerationEndpointService.CreateChapterApproval)
            .WithTags("Moderation")
            .WithName("CreateChapterApproval")
            .WithDescription("Create chapter approval");
        apis.MapPost("chapter/reject", ModerationEndpointService
                .RejectChapter)
            .WithTags("Moderation")
            .WithName("RejectChapter")
            .WithDescription("Reject chapter for book");
        apis.MapGet("chapter-approval", ModerationEndpointService.GetChapterForBook)
            .WithTags("Moderation")
            .WithName("GetChapterHasModerationSuccess")
            .WithDescription("Get chapter approval for book");
        apis.MapGet("book/pagination", ModerationEndpointService.GetBookApprovalWithPagination)
            .WithTags("Moderation")
            .WithName("GetAllBookApprovals")
            .WithDescription("Get all book approvals");
        apis.MapGet("book/chapters", ModerationEndpointService.GetChapterApprovalForBook)
            .WithTags("Moderation")
            .WithName("GetChapterInBookApprovals")
            .WithDescription("Get chapter approval for book");
        apis.MapGet("chapter/detail", ModerationEndpointService.GetChapterApprovalDetail)
            .WithTags("Moderation")
            .WithName("GetChapterDetailForModeration");
        apis.MapGet("chapter/approval/pagination", ModerationEndpointService.GetChapterApprovalPagination)
            .WithTags("Moderation")
            .WithName("GetChapterApprovalPaginationForModeration")
            .WithDescription("Get chapter approval pagination for moderation");
        apis.MapGet("book/ids", ModerationEndpointService.GetBookApprovalByIds)
            .WithTags("Moderation")
            .WithName("GetBookApprovalByIds")
            .WithDescription("Get book approval by ids");
        apis.MapGet("chapter/decision/pagination", ModerationEndpointService.GetApprovalDecisionPagination)
            .WithTags("Moderation")
            .WithName("GetApprovalDecisionPaginationForModeration")
            .WithDescription("Get approval decision pagination for moderation");
    }
}


public static class ModerationEndpointService
{
    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<BookApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        ActiveBook(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] Guid bookId)
    {
        var activeCommand = new ActiveBookApprovalCommand(bookId);
        BookApprovalViewModel bookApprovalViewModel = await services
            .FactoryHandler.Handler<ActiveBookApprovalCommand,
                BookApprovalViewModel>(activeCommand);
        return TypedResults.Ok(bookApprovalViewModel);
    }

    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<BookApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        UnActiveBook(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] Guid bookId
        )
    {
        var unActive = new UnActiveBookApprovalCommand(bookId);
        BookApprovalViewModel bookApprovalViewModel = await services
            .FactoryHandler.Handler<UnActiveBookApprovalCommand,
            BookApprovalViewModel>(unActive);
        return TypedResults.Ok(bookApprovalViewModel);
    }

    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<ChapterApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        ApprovalChapter(
            [FromServices] ModerationServiceWrapper services,
            [AsParameters] ApprovalChapterCommand request
        )
    {
        ChapterApprovalViewModel chapterApprovalViewModel = await services
            .FactoryHandler.Handler<ApprovalChapterCommand, ChapterApprovalViewModel>(request);
        return TypedResults.Ok(chapterApprovalViewModel);
    }

    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<BookApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        CreateBookApproval(
            [FromServices] ModerationServiceWrapper services,
            [AsParameters] CreateBookApprovalCommand request)
    {
        var result = await services.FactoryHandler
            .Handler<CreateBookApprovalCommand, BookApprovalViewModel>(request);
        return TypedResults.Ok(result);
    }

    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<ChapterApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        CreateChapterApproval(
            [FromServices] ModerationServiceWrapper services,
                [AsParameters] CreateChapterApprovalCommand request)
    {
        var result = await services.FactoryHandler
            .Handler<CreateChapterApprovalCommand, ChapterApprovalViewModel>(request);
        return TypedResults.Ok(result);
    }

    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<ChapterApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        RejectChapter(
            [FromServices] ModerationServiceWrapper services,
            [AsParameters] RejectChapterCommand request
        )
    {
        ChapterApprovalViewModel chapterApprovalViewModel = await services
            .FactoryHandler.Handler<RejectChapterCommand, ChapterApprovalViewModel>(request);
        return TypedResults.Ok(chapterApprovalViewModel);
    }
    public static async Task<Results<Ok<IReadOnlyCollection<ChapterStoreViewModel>>, NotFound, UnauthorizedHttpResult>>
        GetChapterForBook(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] Guid bookId
        )
    {
        IReadOnlyCollection<ChapterStoreViewModel> chapterStoreViewModels
            = await services.ModerationQueries
                .GetChaptersForBookAsync(bookId);
        return TypedResults.Ok(chapterStoreViewModels);
    }
    [AuthorizationKey((Role.Admin))]
    public static async Task<Results<Ok<IReadOnlyCollection<ChapterApprovalViewModel>>, NotFound, UnauthorizedHttpResult>>
        GetChapterApprovalForBook(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] Guid bookApprovalId
        )
    {
        IReadOnlyCollection<ChapterApprovalViewModel> chapterStoreViewModels
            = await services.ModerationQueries
                .GetChapterApprovalForBookId(bookApprovalId);
        return TypedResults.Ok(chapterStoreViewModels);
    }
    [AuthorizationKey((Role.Admin))]
    public static async Task<Results<Ok<PaginationItem<BookApprovalViewModel>>, NotFound, UnauthorizedHttpResult>>
        GetBookApprovalWithPagination(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] string? title,
            [AsParameters] PaginationRequest page
        )
    {
        PaginationItem<BookApprovalViewModel> bookApprovals
            = await services.ModerationQueries.GetBookApprovalWithPaginationAsync(title, page);
        return TypedResults.Ok(bookApprovals);
    }
    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<ChapterApprovalViewModel>, NotFound, UnauthorizedHttpResult>>
        GetChapterApprovalDetail(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] Guid chapterApprovalId
            )
    {
        var result = await services.ModerationQueries.GetChapterApprovalDetailAsync(chapterApprovalId);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<PaginationItem<ChapterApprovalViewModel>>, NotFound, UnauthorizedHttpResult>>
        GetChapterApprovalPagination(
            [FromServices] ModerationServiceWrapper services,
            [AsParameters] PaginationRequest page
        )
    {
        var result = await services.ModerationQueries.
            GetChapterNeedModerationWithPaginationAsync(page);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<IReadOnlyCollection<BookApprovalViewModel>>, NotFound, UnauthorizedHttpResult>>
        GetBookApprovalByIds(
            [FromServices] ModerationServiceWrapper services,
            [FromQuery] Guid [] ids
        )
    {
        var result = await services.ModerationQueries.
            GetBookApprovalByIdsAsync(ids);
        return TypedResults.Ok(result);
    }

    [AuthorizationKey(Role.Admin)]
    public static async Task<Results<Ok<PaginationItem<ApprovalDecisionViewModel>>, NotFound, UnauthorizedHttpResult>>
        GetApprovalDecisionPagination(
            [FromQuery] Guid chapterApprovalId,
            [FromServices] ModerationServiceWrapper services,
            [AsParameters] PaginationRequest page
        )
    {
        var result = await services.ModerationQueries
            .GetDecisionForChapterApprovalAsync(chapterApprovalId, page);
        return TypedResults.Ok(result);
    }
}

public class ModerationServiceWrapper(ILogger<ModerationServiceWrapper> logger,
    IIdentityProvider identityProvider,
    IFactoryHandler factoryHandler,
    IModerationQueries moderationQueries)
{
    public IModerationQueries ModerationQueries { get; } = moderationQueries;
    public ILogger<ModerationServiceWrapper> Logger { get; } = logger;
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IFactoryHandler FactoryHandler { get; } = factoryHandler;
}
