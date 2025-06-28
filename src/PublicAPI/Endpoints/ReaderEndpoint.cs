using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.BoundContext.OrderContext.Queries;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Exception;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class ReaderEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("reader");
        apis.MapGet("chapter", ReaderEndpointService.GetContentForChapterSlug)
            .WithTags("Reader")
            .WithName("GetChapterContentForBook")
            .WithDescription("Get chapter content for book");
    }
}


public static class ReaderEndpointService
{
    public static async Task<Results<Ok<string>, NotFound<string>, ForbidHttpResult>>
        GetContentForChapterSlug(
            [FromQuery] string chapterSlug,
            [FromQuery] string bookSlug,
            [FromServices] ReaderServiceWrapper service)
    {
        // If book has buyer but author has deleted find in moderation 
        var bookBySlug = await service.BookAuthoringQueries
            .FindBookBySlugAsync(bookSlug, CancellationToken.None);
        // if book frees
        if (bookBySlug == null)
        {
            return TypedResults.NotFound("Không tìm thấy sách");
        }
        if (bookBySlug.PolicyReadBook.Policy == BookPolicy.Paid)
        {
            var order = await service.OrderQueries
                .GetOrderHasInBookIdAsync(service.IdentityProvider.UserIdentity(),
                    bookBySlug.Id, CancellationToken.None);
            if (order is null || order.Status != OrderStatus.Success)
            {
                return TypedResults.Forbid();           
            }
        }

        var chapterContent = await service
            .ModerationQueries.GetContentForChapterAsync(chapterSlug);
        return TypedResults.Ok(chapterContent);
    }
}

public class ReaderServiceWrapper(
    IIdentityProvider identityProvider,
    IFactoryHandler factoryHandler,
    ILogger<ReaderServiceWrapper> logger,
    IBookAuthoringQueries bookAuthoringQueries,
    IModerationQueries moderationQueries,
    IOrderQueries orderQueries,
    IValidationServices<BookApproval> bookApprovalValidationService)
{
    public IValidationServices<BookApproval> BookApprovalValidationService { get; } = bookApprovalValidationService;
    public IModerationQueries ModerationQueries { get; } =  moderationQueries;
    public IBookAuthoringQueries BookAuthoringQueries { get; } = bookAuthoringQueries;
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IFactoryHandler FactoryHandler { get; } = factoryHandler;
    public ILogger<ReaderServiceWrapper> Logger { get; } = logger;
    public IOrderQueries OrderQueries { get; } =  orderQueries;
}
