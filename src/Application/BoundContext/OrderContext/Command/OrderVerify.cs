using Application.BoundContext.OrderContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Payment;
using Application.Models.Payment;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record OrderVerifyCommand(VerifyPaymentRequest verifyPaymentRequest)
    : IOrderCommand<OrderViewModel>;

public class OrderVerifyCommandHandler(
    ILogger<OrderVerifyCommand> logger,
    IOrderRepository orderRepository,
    IPayment payment)
    : ICommandHandler<OrderVerifyCommand, OrderViewModel>
{
    private readonly ILogger<OrderVerifyCommand> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPayment _payment = payment;
    public async Task<OrderViewModel> Handle(OrderVerifyCommand request, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(request.verifyPaymentRequest.OrderId, out Guid orderId))
        {   
            throw new Exception($"Not find order has id {request.verifyPaymentRequest.OrderId}");
        }
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
        var verifyPayment = _payment.VerifyPaymentResponse(request.verifyPaymentRequest);
        if (verifyPayment)
        {
            order.OrderSuccess();
        }
        else
        {
            order.OrderFailed();
        }
        _orderRepository.Update(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return order.ToViewModel();
    }
}
