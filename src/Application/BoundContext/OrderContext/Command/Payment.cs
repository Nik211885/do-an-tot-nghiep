using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.BoundContext.OrderContext.Message;
using Application.Exceptions;
using Application.Interfaces.Clients;
using Application.Interfaces.CQRS;
using Application.Interfaces.Payment;
using Application.Models.Payment;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record PaymentCommand(Guid OrderId, string ReturnUrl)
    : IOrderCommand<PaymentResponse>;

public class PaymentCommandHandler(
    ILogger<PaymentCommandHandler> logger,
    IOrderRepository orderRepository,
    IPayment payment,
    ICheckClientAddressAppServices checkClientAddressAppServices,
    IBookAuthoringQueries bookAuthoringQueries)
    : ICommandHandler<PaymentCommand, PaymentResponse>
{
    private readonly ILogger<PaymentCommandHandler> _logger = logger;
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IPayment _payment = payment;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    private readonly ICheckClientAddressAppServices _checkClientAddressAppServices = checkClientAddressAppServices;
    public async Task<PaymentResponse> Handle(PaymentCommand request, CancellationToken cancellationToken)
    {
        if (!_checkClientAddressAppServices.IsClientAddress(request.ReturnUrl))
        {
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.ReturnUrlInvalid);
        }
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
        order.Payment();
        // Cap nhat lai gia sach
        var task = new List<Task<BookViewModel>>();
        foreach (var o in order.OrderItems)
        {
            task.Add(_bookAuthoringQueries.FindBookByIdAsync(o.BookId, cancellationToken)!);
        }

        var bookViewModels = await Task.WhenAll(task);
        foreach (var o in order.OrderItems)
        {
            var bookViewModel =  bookViewModels.FirstOrDefault(b => b.Id == o.BookId);
            if (bookViewModel is null 
                || bookViewModel.PolicyReadBook.Policy == BookPolicy.Free
                || bookViewModel.PolicyReadBook.Price is null
                )
            {
                order.RemoveOrderItem(o.BookId);
            }
            else
            {
                order.ChangPriceForOrderItem(o.BookId, (decimal)bookViewModel.PolicyReadBook.Price);
            }
        }
        if (order.OrderItems.Count == 0)
        {
            _orderRepository.Delete(order);
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.DontHaveBookInOrder);
        }
        else
        {
            _orderRepository.Update(order);
        }
        // Update price for book authoring
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
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return paymentResponse;
    }
}
