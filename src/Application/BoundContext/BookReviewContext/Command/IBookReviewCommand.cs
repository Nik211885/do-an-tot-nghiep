using Application.Interfaces.CQRS;

namespace Application.BoundContext.BookReviewContext.Command;

public interface IBookReviewCommand<TResponse>
    : ICommand<TResponse>;
