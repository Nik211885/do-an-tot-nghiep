namespace Application.Interfaces.CQRS;

/// <summary>
///     Handler for command
/// </summary>
/// <typeparam name="TCommand">Request</typeparam>
/// <typeparam name="TResponse">Response</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse> { }
    
