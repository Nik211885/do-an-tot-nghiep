using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Models;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class UnActivatedBookDomainEventHandler(
    ILogger<UnActivatedBookDomainEventHandler> logger,
    IElasticServices<BookElasticModel> bookElasticServices)
    : IEventHandler<UnActivatedBookDomainEvent>
{
    private readonly ILogger<UnActivatedBookDomainEventHandler> _logger = logger;
    private readonly IElasticServices<BookElasticModel> _bookElasticServices = bookElasticServices;
    public async Task Handler(UnActivatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Started turn off active for book has id {Id}", domainEvent.BookApproval.BookId);
        BookElasticModel? bookElastic = await _bookElasticServices.GetAsync(domainEvent.BookApproval.BookId);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookElastic, "Sach");
        bookElastic.IsActive = false;
        await _bookElasticServices.UpdateAsync(bookElastic);
    }
}
