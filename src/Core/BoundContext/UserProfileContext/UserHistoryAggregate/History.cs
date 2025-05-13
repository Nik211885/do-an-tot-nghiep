using Core.Entities;
using Core.Interfaces;

namespace Core.BoundContext.UserProfileContext.UserHistoryAggregate;

public class History : BaseEntity, 
    IAggregateRoot
{
    public Guid UserId { get; private set; }
}
