using Application.BoundContext.BookAuthoringContext.Queries;
using Application.BoundContext.OrderContext.Queries;
using Application.BoundContext.OrderContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Application.Models;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.BoundContext.OrderContext.OrderAggregate;
using Core.Interfaces.Repositories.OrderContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.OrderContext.Command;

public record CreateOrderWithOrderItemCommand(Guid BookId)
    : IOrderCommand<OrderViewModel>;

public class CreateOrderWithOrderItemCommandHandler(IOrderRepository orderRepository,
    ILogger<CreateOrderWithOrderItemCommandHandler> logger, 
    IElasticServices<BookElasticModel> elasticServices,
    IValidationServices<Book> bookAuthoringValidation,
    IIdentityProvider identityProvider,
    IOrderQueries orderQueries)
    : ICommandHandler<CreateOrderWithOrderItemCommand, OrderViewModel>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly ILogger<CreateOrderWithOrderItemCommandHandler> _logger = logger;
    private readonly IElasticServices<BookElasticModel> _elasticServices = elasticServices;
    private readonly IValidationServices<Book> _bookAuthoringValidation = bookAuthoringValidation;
    private readonly IOrderQueries _orderQueries = orderQueries;
    public async Task<OrderViewModel> Handle(CreateOrderWithOrderItemCommand request, CancellationToken cancellationToken)
    {
        var book = await _elasticServices.GetAsync(request.BookId.ToString());
        ThrowHelper.ThrowNotFoundWhenItemIsNull(book, "Sách");
        var bookInAuthoring = await _bookAuthoringValidation.AnyAsync(x=>x.Id == request.BookId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookInAuthoring, "Sách");
        // Find in moderation
        if (bookInAuthoring.PolicyReadBook.Policy != BookPolicy.Paid)
        {
            ThrowHelper.ThrowIfBadRequest("Sách này không phải trả phí");
        }
        var orderExits = await _orderQueries.GetOrderHasInBookIdAsync(_identityProvider.UserIdentity(), request.BookId, cancellationToken);
        if (orderExits is not null)
        {
            return orderExits;
        }
        var order = Order.Create(_identityProvider.UserIdentity());
        order.AddOrderItem(bookInAuthoring.Id, bookInAuthoring.Title, bookInAuthoring.PolicyReadBook.Price ?? 0, bookInAuthoring.CreatedUerId);
        _orderRepository.Create(order);
        await _orderRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return order.ToViewModel();
    }
}
