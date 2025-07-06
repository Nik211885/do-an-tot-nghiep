using Application.BoundContext.OrderContext.Command;
using Application.BoundContext.OrderContext.Queries;
using Application.BoundContext.OrderContext.ViewModel;
using Application.Common;
using Application.Common.Authorization;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Models;
using Application.Models.Payment;
using Core.BoundContext.OrderContext.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PublicAPI.Services.Endpoint;

namespace PublicAPI.Endpoints;

public class OrderEndpoint : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var api = endpoint.MapGroup("order"); 
        api.MapPost("create", OrderEndpointServices.CreateOrder)
            .WithTags("Order")
            .WithName("CreateOrder")
            .WithDescription("Create new Order");
        api.MapDelete("delete",  OrderEndpointServices.DeleteOrder)
            .WithTags("Order")
            .WithName("DeleteOrder")
            .WithDescription("Delete Order");
        api.MapPost("new-order-item",  OrderEndpointServices.CreateOrderItem)
            .WithTags("Order")
            .WithName("CreateOrderItem")
            .WithDescription("Create new Order Item");
        api.MapPut("remove-order-item",   OrderEndpointServices.RemoveOrderItem)
            .WithTags("Order")
            .WithName("RemoveOrderItem")
            .WithDescription("Remove Order Item");
        api.MapGet("payment", OrderEndpointServices.PaymentOrder)
            .WithTags("Order")
            .WithName("PaymentForOrder")
            .WithDescription("Payment for Order");
        api.MapGet("payment/callback", OrderEndpointServices.VerifyOrder)
            .WithTags("Order")
            .WithName("VerifyPaymentForOrder")
            .WithDescription("Verify Payment for Order");
        api.MapGet("for-my/pagination", OrderEndpointServices.GetOrderForMyWithPagination)
            .WithTags("Order")
            .WithName("GetOrderForMyWithPagination")
            .WithDescription("Get Order For My With Pagination");
        api.MapPost("create-order/v1", OrderEndpointServices.CreateOrderWithOrderItem)
            .WithTags("Order")
            .WithName("CreateOrderWithOrderItem")
            .WithDescription("Create new Order Item");
        api.MapGet("my-order/book-in", OrderEndpointServices.GetOrderItemInIdsAndStatus)
            .WithTags("Order")
            .WithName("GetMyOrderItemInIdsAndStatus")
            .WithDescription("Get my order item In Ids and Status");
    }
}


public static class OrderEndpointServices
{
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<OrderViewModel>, ProblemHttpResult>>
        CreateOrder(
            [FromServices] OrderServiceWrapper service
        )
    {
        var command = new CreateOrderCommand();
        var result = await service.FactoryHandler.Handler<CreateOrderCommand, OrderViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<NoContent, ProblemHttpResult, NotFound>>
        DeleteOrder(
            [AsParameters] DeleteOrderCommand command,
            [FromServices] OrderServiceWrapper service
        )
    {
        await service.FactoryHandler.Handler<DeleteOrderCommand, bool>(command);
        return TypedResults.NoContent();
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<OrderViewModel>, ProblemHttpResult, NotFound>>
        CreateOrderItem(
            [FromBody] CreateOrderItemCommand command,
            [FromServices] OrderServiceWrapper service
        )
    {
        var result = await service.FactoryHandler.Handler<CreateOrderItemCommand, OrderViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<OrderViewModel>, ProblemHttpResult, NotFound>>
        RemoveOrderItem(
            [FromBody] RemoveOrderItemCommand command,
            [FromServices] OrderServiceWrapper service
        )
    {
        var result = await service.FactoryHandler.Handler<RemoveOrderItemCommand, OrderViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<PaymentResponse>, ProblemHttpResult, NotFound>>
        PaymentOrder(
            [AsParameters] PaymentCommand command,
            [FromServices] OrderServiceWrapper service
        )
    {
        var result = await service.FactoryHandler.Handler<PaymentCommand,PaymentResponse>(command);
        return TypedResults.Ok(result);
    }
    public static async Task<Results<RedirectHttpResult, ProblemHttpResult, NotFound>>
        VerifyOrder(
            [AsParameters] VerifyPaymentRequest request,
            [FromServices] OrderServiceWrapper service
        )
    {
        
        var command = new OrderVerifyCommand(request);
        var (extraUrl,order) = await service.FactoryHandler.Handler<OrderVerifyCommand,(string,OrderViewModel)>(command);
        var url = $"{extraUrl}?OrderId={order.Id}";
        return TypedResults.Redirect(url);
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<PaginationItem<OrderViewModel>>, ProblemHttpResult, NotFound>>
        GetOrderForMyWithPagination(
            [AsParameters] PaginationRequest page,
            [FromServices] OrderServiceWrapper service
        )
    {
        var result = await service.OrderQueries.GetOrderForUserWithPaginationAsync(service.IdentityProvider.UserIdentity(), page);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<OrderViewModel>, BadRequest, NotFound>>
        CreateOrderWithOrderItem(
            [FromBody] CreateOrderWithOrderItemCommand command,
            [FromServices] OrderServiceWrapper service
        )
    {
        var result = await service.FactoryHandler
            .Handler<CreateOrderWithOrderItemCommand, OrderViewModel>(command);
        return TypedResults.Ok(result);
    }
    [AuthorizationKey(Role.Author)]
    public static async Task<Results<Ok<IReadOnlyCollection<OrderViewModel>>, BadRequest, NotFound>>
        GetOrderItemInIdsAndStatus(
            [FromQuery] Guid[] bookId,
            [FromQuery] OrderStatus? status,
            [FromServices] OrderServiceWrapper 
                service
        )
    {
        var result = await service.OrderQueries
            .GetAllOrderByBookIdsAsync(bookId,
                service.IdentityProvider.UserIdentity(),
                status);
        return TypedResults.Ok(result);
    }
}


public class OrderServiceWrapper(ILogger<OrderServiceWrapper> logger,
    IOrderQueries orderQueries,
    IFactoryHandler factoryHandler,
    IIdentityProvider identityProvider)
{
    public ILogger<OrderServiceWrapper> Logger { get; } = logger;
    public IOrderQueries OrderQueries { get; } = orderQueries;
    public IIdentityProvider IdentityProvider { get; } = identityProvider;
    public IFactoryHandler FactoryHandler { get; } = factoryHandler;
}
