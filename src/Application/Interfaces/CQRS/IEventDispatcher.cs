﻿using Core.Events;

namespace Application.Interfaces.CQRS;
/// <summary>
/// 
/// </summary>
public interface IEventDispatcher
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="events"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Dispatch<TEvent>(IReadOnlyCollection<TEvent> events, CancellationToken cancellationToken)
        where TEvent : IEvent;
}
