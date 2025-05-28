using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Events;
using Core.Events.BookAuthoringContext;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class CacheInvalidationBookDomainHandler(ICache cache, IIdentityProvider identityProvider)
    : IEventHandler<CreatedBookDomainEvent>,
        IEventHandler<BookChangedReleaseTypeDomainEvent>,
        IEventHandler<UpdatedBookDomainEvent>,
        IEventHandler<BookUpdatePolicyReaderBookDomainEvent>,
        IEventHandler<DeletedBookDomainEvent>
{
    private readonly ICache _cache = cache;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task Handler(CreatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await InvalidationCache(domainEvent.UserId);
    }

    public async Task Handler(BookChangedReleaseTypeDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await InvalidationCache(domainEvent.UserId);
    }

    public async Task Handler(UpdatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await InvalidationCache(domainEvent.UserId);
    }

    public async Task Handler(BookUpdatePolicyReaderBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await InvalidationCache(domainEvent.UserId);
    }

    public async Task Handler(DeletedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await InvalidationCache(domainEvent.UserId);
    }

    private async Task InvalidationCache(Guid userId)
    {
        var cacheKey = string.Format(CacheKey.BookByUserId, userId);
        await _cache.RemoveAsync(cacheKey);
    }
}
