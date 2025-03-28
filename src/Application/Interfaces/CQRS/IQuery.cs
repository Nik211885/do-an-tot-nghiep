namespace Application.Interfaces.CQRS;

/// <summary>
///     Defined type for action query it make change source data and defined type of data response 
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<TResponse> : IRequest<TResponse> { }
