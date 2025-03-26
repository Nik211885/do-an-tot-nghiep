namespace Application.Interfaces.CQRS;
/// <summary>
/// 
/// </summary>
/// <typeparam name="TEvent"></typeparam>
public interface IEventDispatcher<TEvent>
    where TEvent : notnull
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Dispatch(CancellationToken cancellationToken);
}
