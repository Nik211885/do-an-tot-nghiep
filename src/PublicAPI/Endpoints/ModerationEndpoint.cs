using Application.BoundContext.BookReviewContext.ViewModel;
using Application.BoundContext.ModerationContext.Command;
using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Common;
using Application.Common.Authorization;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
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
        apis.MapGet("chapter/approval", ModerationEndpointService.ApprovalChapter)
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
        apis.MapPut("chapter/rejecte", ModerationEndpointService
                .RejectChapter)
            .WithTags("Moderation")
            .WithName("RejectChapter")
            .WithDescription("Reject chapter for book");
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
