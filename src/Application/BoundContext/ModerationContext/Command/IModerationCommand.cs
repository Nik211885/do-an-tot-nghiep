using Application.Interfaces.CQRS;

namespace Application.BoundContext.ModerationContext.Command;

public interface IModerationCommand<TResponse>
    : ICommand<TResponse>;
