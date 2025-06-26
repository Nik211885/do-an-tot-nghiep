    using Application.BoundContext.OrderContext.ViewModel;
    using Application.Exceptions;
    using Application.Interfaces.CQRS;
    using Application.Interfaces.Payment;
    using Application.Models.Payment;
    using Core.Interfaces.Repositories.OrderContext;
    using Microsoft.Extensions.Logging;

    namespace Application.BoundContext.OrderContext.Command;

    public record OrderVerifyCommand(VerifyPaymentRequest VerifyPaymentRequest)
        : IOrderCommand<(string, OrderViewModel)>;

    public class OrderVerifyCommandHandler(
        ILogger<OrderVerifyCommand> logger,
        IOrderRepository orderRepository,
        IPayment payment)
        : ICommandHandler<OrderVerifyCommand, (string,OrderViewModel)>
    {
        private readonly ILogger<OrderVerifyCommand> _logger = logger;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IPayment _payment = payment;
        public async Task<(string,OrderViewModel)> Handle(OrderVerifyCommand request, CancellationToken cancellationToken)
        {
            if (request.VerifyPaymentRequest.OrderId is null)
            {
                ThrowHelper.ThrowIfBadRequest("Khong tim thay don dat");
            }
            var order = await _orderRepository.GetOrderByCodeAsync(request.VerifyPaymentRequest.OrderId!, cancellationToken);
            ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
            var verifyPayment = _payment.VerifyPaymentResponse(request.VerifyPaymentRequest);
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
            return (request.VerifyPaymentRequest.ExtraData!,order.ToViewModel());
        }
    }
