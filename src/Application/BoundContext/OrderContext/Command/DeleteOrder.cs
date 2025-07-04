using Application.BoundContext.OrderContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record DeleteOrderCommand(Guid OrderId)
    : IOrderCommand<bool>;

public class DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
    : ICommandHandler<DeleteOrderCommand, bool>
{
    private readonly ILogger<DeleteOrderCommandHandler> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order,"Don dat");
        order.Delete();
        _orderRepository.Delete(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return true;
    }
}
