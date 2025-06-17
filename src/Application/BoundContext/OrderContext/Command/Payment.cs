using Application.BoundContext.OrderContext.Message;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Payment;
using Application.Models.Payment;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record PaymentCommand(Guid OrderId)
    : IOrderCommand<PaymentResponse>;

public class PaymentCommandHandler(
    ILogger<PaymentCommandHandler> logger,
    IOrderRepository orderRepository,
    IPayment payment)
    : ICommandHandler<PaymentCommand, PaymentResponse>
{
    private readonly ILogger<PaymentCommandHandler> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPayment _payment = payment;
    public async Task<PaymentResponse> Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
        if (order.OrderItems.Count == 0)
        {
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.DontHaveBookInOrder);
        }

        var paymentRequest = new PaymentRequest(
                order.CalculatePrice(),
                order.Id.ToString(),
                "Thanh toán đơn sách",
                PaymentMethod.Credit,
                "vi"
            );
        var paymentResponse = await _payment.CreatePaymentRequestAsync(paymentRequest, cancellationToken);
        return paymentResponse;

    }
}
