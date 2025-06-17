using Application.Interfaces.CQRS;

namespace Application.BoundContext.OrderContext.Command;

public interface IOrderCommand<TResponse>
    : ICommand<TResponse>;
