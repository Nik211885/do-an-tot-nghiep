namespace Application.Interfaces.CQRS;

/// <summary>
///     Base handler for command and query
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    /// <summary>
    ///     Defined handler for command and query
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
