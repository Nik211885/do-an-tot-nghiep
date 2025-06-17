using Application.BoundContext.OrderContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record CreateOrderCommand
    : IOrderCommand<OrderViewModel>;

public class CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IOrderRepository orderRepository, IIdentityProvider identityProvider)
    : ICommandHandler<CreateOrderCommand, OrderViewModel>
{
    private readonly ILogger<CreateOrderCommandHandler> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<OrderViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(_identityProvider.UserIdentity());
        _orderRepository.Create(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return order.ToViewModel(); 
    }
}
