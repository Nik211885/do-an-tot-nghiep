namespace Application.Interfaces.CQRS;

/// <summary>
///     Handler for query
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse> { }
