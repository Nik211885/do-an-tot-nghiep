using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.BoundContext.OrderContext.Message;
using Application.BoundContext.OrderContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Models;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record CreateOrderItemCommand(Guid OrderId, Guid BookId)
    : IOrderCommand<OrderViewModel>;

public class CreateOrderItemCommandHandler(
    IOrderRepository orderRepository,
    ILogger<CreateOrderItemCommandHandler> logger,
    IBookAuthoringQueries bookAuthoringQueries,
    IElasticServices<BookElasticModel> bookElasticServices)
    : ICommandHandler<CreateOrderItemCommand, OrderViewModel>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    private readonly IElasticServices<BookElasticModel> _bookElasticServices = bookElasticServices;
    private readonly ILogger<CreateOrderItemCommandHandler> _logger = logger;
    public async Task<OrderViewModel> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var bookActive = await _bookElasticServices
            .GetAsync(request.BookId);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookActive, "Sach");
        /*if (!bookActive.IsActive)
        {
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.BookDontHasPaid);
        }*/
        // In fact need check book has censor for system 
        // If has payment for order 
        // notification for order has buyer
        var book = await _bookAuthoringQueries
            .FindBookByIdAsync(request.BookId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book, "Sach");
        if (book.PolicyReadBook.Policy != BookPolicy.Paid
            || book.PolicyReadBook.Price is null)
        {
            ThrowHelper.ThrowIfBadRequest(OrderValidationMessage.BookDontHasPaid);
        }
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(order, "Don dat");
        order.AddOrderItem(book.Id, book.Title, book.PolicyReadBook.Price ?? 0, book.AuthorId);
        _orderRepository.Update(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return order.ToViewModel();
    }
}

