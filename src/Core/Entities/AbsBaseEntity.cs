﻿using Core.Events;

namespace Core.Entities;
/// <summary>
///     Base entity in core just have id and event domain
/// </summary>
public abstract class AbsBaseEntity
{
    /// <summary>
    ///     Identity for entity it follows id is uuid7
    /// </summary>
    public Guid Id { get; private set; } = Guid.CreateVersion7();

    /// <summary>
    ///     
    /// </summary>
    private List<IEvent>? _events;
    /// <summary>
    ///     
    /// </summary>
    /// <param name="event"></param>
    public void RaiseDomainEvent(IEvent @event)
    {
        _events ??= [];
        _events.Add(@event);
    }

    /// <summary>
    ///     
    /// </summary>
    public IReadOnlyCollection<IEvent>? DomainEvents
        => _events?.AsReadOnly();
    /// <summary>
    ///     
    /// </summary>
    public void ClearDomainEvents() => _events?.Clear();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="event"></param>
    public void RemoveDomainEvent(IEvent @event)
        => _events?.Remove(@event);
    
    /// <summary>
    ///     If two entity considered equal when it has same id 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>
    ///     Return true if two object has equal
    /// </returns>
    public override bool Equals(object? obj)
    {
        // if object not is entity return false it just compare entity with entity
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        // if object has reference address obj
        if (Object.ReferenceEquals(this, obj))
        {
            return true;
        }

        if (this.GetType() != obj.GetType())
        {
            return false;
        }
        AbsBaseEntity absBaseEntity = (AbsBaseEntity)obj;
        return absBaseEntity.Id == this.Id;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="entityCompare"></param>
    /// <returns></returns>
    public static bool operator == (AbsBaseEntity entity, AbsBaseEntity entityCompare)
    {
        if (Object.Equals(entity, null))
        {
            return (Object.Equals(entityCompare, null) ? true : false);
        }
        return entity.Equals(entityCompare);
    }
    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <param name="entity"></param>
    /// <param name="entityCompare"></param>
    /// <returns></returns>
    public static bool operator !=(AbsBaseEntity entity, AbsBaseEntity entityCompare)
        => !(entity == entityCompare);
    /// <summary>
    ///  Hash entity to number it make compare entity faster
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
        => base.GetHashCode();
}
