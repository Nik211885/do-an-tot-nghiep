using Application.BoundContext.OrderContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record RemoveOrderItemCommand(Guid OrderId, Guid OrderItemId)
    : IOrderCommand<OrderViewModel>;

public class RemoveOrderItemCommandHandler(
    IOrderRepository orderRepository,
    ILogger<RemoveOrderItemCommandHandler> logger)
    : ICommandHandler<RemoveOrderItemCommand, OrderViewModel>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly ILogger<RemoveOrderItemCommandHandler> _logger = logger;
    public async Task<OrderViewModel> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
        order.RemoveOrderItem(request.OrderId);
        _orderRepository.Update(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return order.ToViewModel();
    }
}

