namespace Application.Interfaces.CQRS;

/// <summary>
///     Factory command or query handler and execute task
/// </summary>
public interface IFactoryHandler
{
    /// <summary>
    ///     Factory command or query handler execute task
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns>
    ///     Throw argument exception null if services don't find handler
    /// </returns>
    Task<TResponse> Handler<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>;
}
