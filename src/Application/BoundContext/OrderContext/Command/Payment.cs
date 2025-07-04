using Application.BoundContext.OrderContext.Message;
using Application.Exceptions;
using Application.Interfaces.Clients;
using Application.Interfaces.CQRS;
using Application.Interfaces.Payment;
using Application.Models.Payment;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record PaymentCommand(Guid OrderId, string ReturnUrl)
    : IOrderCommand<PaymentResponse>;

public class PaymentCommandHandler(
    ILogger<PaymentCommandHandler> logger,
    IOrderRepository orderRepository,
    IPayment payment,
    ICheckClientAddressAppServices checkClientAddressAppServices)
    : ICommandHandler<PaymentCommand, PaymentResponse>
{
    private readonly ILogger<PaymentCommandHandler> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPayment _payment = payment;
    private readonly ICheckClientAddressAppServices _checkClientAddressAppServices = checkClientAddressAppServices;
    public async Task<PaymentResponse> Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        if (!_checkClientAddressAppServices.IsClientAddress(request.ReturnUrl))
        {
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.ReturnUrlInvalid);
        }
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
        if (order.OrderItems.Count == 0)
        {
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.DontHaveBookInOrder);
        }
        order.Payment();
        // If  order has success
        var paymentRequest = new PaymentRequest(
                order.CalculatePrice(),
                order.OrderCode,
                $"Thanh toán hóa đơn mua sách",
                PaymentMethod.Credit,
                "vi",
                extraData: request.ReturnUrl
            );
        var paymentResponse = await _payment.CreatePaymentRequestAsync(paymentRequest, cancellationToken);
        _orderRepository.Update(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return paymentResponse;

    }
}
