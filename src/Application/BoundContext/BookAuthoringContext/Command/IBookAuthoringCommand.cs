using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookAuthoringContext.Command;

public interface IBookAuthoringCommand<TCommand> : ICommand<TCommand>;
