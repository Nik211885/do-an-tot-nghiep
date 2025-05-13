namespace Core.Events.UserProfileContext;

public class UserFollowedDomainEvent(Guid followerId, Guid monitorId) : IEvent
{
    public Guid FollowerId { get; } = followerId;
    public Guid MonitorId { get; } = monitorId;
}
