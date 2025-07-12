using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Models;
using Core.Events.ModerationContext;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class ActivatedBookDomainEventHandler(
    ILogger<ActivatedBookDomainEventHandler> logger,
    IElasticServices<BookElasticModel> bookElasticServices)
    : IEventHandler<ActivatedBookDomainEvent>
{
    private readonly ILogger<ActivatedBookDomainEventHandler> _logger = logger;
    private readonly IElasticServices<BookElasticModel> _bookElasticServices = bookElasticServices;
    public async Task Handler(ActivatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Un active for book has id {@Id}", domainEvent.BookApproval.BookId);
        var bookApproval =await _bookElasticServices.GetAsync(domainEvent.BookApproval.BookId);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Sach");
        bookApproval.IsActive = true;
        await _bookElasticServices.UpdateAsync(bookApproval);
    }
}
