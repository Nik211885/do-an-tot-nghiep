namespace Application.Interfaces.CQRS;

/// <summary>   
///     Defined type for action command it make change source data and defined type of data response 
/// </summary>  
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<TResponse> : IRequest<TResponse> { }

