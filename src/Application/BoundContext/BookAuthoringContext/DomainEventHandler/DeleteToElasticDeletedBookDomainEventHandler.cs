using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Models;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class DeleteToElasticDeletedBookDomainEventHandler(
    ILogger<DeleteToElasticDeletedBookDomainEventHandler> logger,
    IElasticFactory elasticFactory)
    : IEventHandler<DeletedBookDomainEvent>
{
    private readonly ILogger<DeleteToElasticDeletedBookDomainEventHandler> _logger = logger; 
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    public async Task Handler(DeletedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var elasticForBook = _elasticFactory.GetInstance<BookElasticModel>();

        var bookElastic = await elasticForBook.GetAsync(domainEvent.Book.Id.ToString());
        if ((bookElastic is null))
        {
            _logger.LogError("Can't not find book has id {Id} in elastic", domainEvent.Book.Id);
            return;
        }
        await elasticForBook.DeleteAsync(bookElastic);
        _logger.LogInformation("Deleted book to elastic {@BookId}", domainEvent.Book.Id);
    }
}
