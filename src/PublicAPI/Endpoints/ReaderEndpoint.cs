using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookReviewContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.BoundContext.OrderContext.Queries;
using Application.BoundContext.OrderContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
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
    public static async Task<Results<Ok<string>, NotFound, ForbidHttpResult>>
        GetContentForChapterSlug(
            [FromQuery] string chapterSlug,
            [FromQuery] Guid bookId,
            [FromServices] ReaderServiceWrapper service)
    {
        // If book has buyer but author has deleted find in moderation 
        var bookById = await service.BookAuthoringQueries
            .FindBookByIdAsync(bookId, CancellationToken.None);
        // if book frees
        // Find order 
        // If book is free 
        if(bookById is { PolicyReadBook.Policy: BookPolicy.Free })
        {
            var chapterContent = await service
                .ModerationQueries.GetContentForChapterAsync(bookById.Id,  chapterSlug, CancellationToken.None);
            await service.EventBus.Publish(new ReadeBookIntegrationEvent(bookId, service.IdentityProvider.UserIdentity()));
            return TypedResults.Ok(chapterContent);
        }
        // If book is not free
        var order = await service.OrderQueries
            .GetOrderHasInBookIdAsync(service.IdentityProvider.UserIdentity(),
                bookId, CancellationToken.None);
        // If author has deleted
        if (bookById is null)
        {
            // If user has buyer success
            if (order is null || order.Status != OrderStatus.Success)
            {
                return TypedResults.NotFound();
            }
            else
            {
                //  return for content
                var chapterContent = await service
                    .ModerationQueries.GetContentForChapterAsync(bookId,  chapterSlug, CancellationToken.None);
                await service.EventBus.Publish(new ReadeBookIntegrationEvent(bookId, service.IdentityProvider.UserIdentity()));
                return TypedResults.Ok(chapterContent);
            }
        }
        // When book has exits in book authoring context for author
        else
        {
            // In top key has check if book has free 
            // In here just pass when book has paid
            // So will just check order has exits for user
            if (order is null || order.Status != OrderStatus.Success)
            {
                return TypedResults.Forbid();
            }
            else
            {
                var chapterContent = await service
                    .ModerationQueries.GetContentForChapterAsync(bookId,  chapterSlug, CancellationToken.None);
                await service.EventBus.Publish(new ReadeBookIntegrationEvent(bookId, service.IdentityProvider.UserIdentity()));
                return TypedResults.Ok(chapterContent);
            }
        }
    }
}

public class ReaderServiceWrapper(
    IIdentityProvider identityProvider,
    IFactoryHandler factoryHandler,
    ILogger<ReaderServiceWrapper> logger,
    IBookAuthoringQueries bookAuthoringQueries,
    IModerationQueries moderationQueries,
    IOrderQueries orderQueries,
    IValidationServices<BookApproval> bookApprovalValidationService,
    IEventBus<ReadeBookIntegrationEvent> eventBus)
{
    public IValidationServices<BookApproval> BookApprovalValidationService { get; } = bookApprovalValidationService;
    public IModerationQueries ModerationQueries { get; } =  moderationQueries;
    public IBookAuthoringQueries BookAuthoringQueries { get; } = bookAuthoringQueries;
    public IEventBus<ReadeBookIntegrationEvent> EventBus { get; } = eventBus;
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IFactoryHandler FactoryHandler { get; } = factoryHandler;
    public ILogger<ReaderServiceWrapper> Logger { get; } = logger;
    public IOrderQueries OrderQueries { get; } =  orderQueries;
}
