using Application.BoundContext.NotificationContext.Command;
using Application.BoundContext.NotificationContext.Queries;
using Application.BoundContext.NotificationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class NotificationEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("notification");
        apis.MapPost("create", NotificationEndpointServices.CreateNotification)
            .WithTags("Notification")
            .WithName("CreateNotification")
            .WithDescription("Create Notification");
        apis.MapPost("mark-read", NotificationEndpointServices.MarkReadNotification)
            .WithTags("Notification")
            .WithName("MarkReadNotification")
            .WithDescription("Mark Read Notification");
        apis.MapGet("all", NotificationEndpointServices.GetNotificationForUserWithPagination)
            .WithTags("Notification")
            .WithName("GetAllNotifications")
            .WithDescription("GetAll Notifications");
        apis.MapPost("mark--all-read-for-user", NotificationEndpointServices.MarkAllNotificationHasReadForUser)
            .WithTags("Notification")
            .WithName("MarkAllNotificationsWithReadForUser")
            .WithDescription("Mark All Notifications with Read For User");
    }
}

public static class NotificationEndpointServices
{
    [Authorize]
    public static async Task<Results<Ok<NotificationViewModel>, ProblemHttpResult>> CreateNotification(
        [FromBody] CreateNotificationCommand command,
        [FromServices] NotificationServicesWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<CreateNotificationCommand, NotificationViewModel>(command);
        return TypedResults.Ok(result);
    }
    
    [Authorize]
    public static async Task<Results<NoContent, ProblemHttpResult>> MarkReadNotification(
        [FromBody] NotificationMarkReadCommand command,
        [FromServices] NotificationServicesWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<NotificationMarkReadCommand, bool>(command);
        return TypedResults.NoContent();
    }
    [Authorize]
    public static async Task<Results<Ok<PaginationItem<NotificationViewModel>>, ProblemHttpResult>> GetNotificationForUserWithPagination(
        [FromQuery] Guid userId, [AsParameters]  PaginationRequest page,
        [FromServices] NotificationServicesWrapper service
    )
    {
        var result = await service
            .ModerationQueries
            .GetNotificationForUserWithPaginationAsync(userId, page);
        return TypedResults.Ok(result);
    }
    [Authorize]
    public static async Task<Results<NoContent, ProblemHttpResult>> MarkAllNotificationHasReadForUser(
        [AsParameters] MarkAllNotificationNotReadForUserCommand command,
        [FromServices] NotificationServicesWrapper service
    )
    {
        var result = await service.FactoryHandler.Handler<MarkAllNotificationNotReadForUserCommand, bool>(command);
        return TypedResults.NoContent();
    }
}

public class NotificationServicesWrapper(IFactoryHandler factoryHandler,
    ILogger<BookAuthoringServiceWrapper> logger,
    INotificationQueries notificationQueries)
{
    public INotificationQueries ModerationQueries { get; } = notificationQueries;
    public IFactoryHandler FactoryHandler { get;} = factoryHandler;
    public ILogger<BookAuthoringServiceWrapper> Logger { get;} = logger;
}
