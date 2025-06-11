using Application.BoundContext.BookAuthoringContext.Queries;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.IdentityProvider;
using Application.Models;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class UpdateToElasticUpdatedBookDomainEventHandler(
    ILogger<UpdateToElasticUpdatedBookDomainEventHandler> logger,
    IElasticFactory elasticFactory,
    IBookAuthoringQueries bookAuthoringQueries,
    IIdentityProvider identityProvider)
    : IEventHandler<UpdatedBookDomainEvent>,
        IEventHandler<AddedGenreForBookDomainEvent>,
        IEventHandler<BookChangedReleaseTypeDomainEvent>,
        IEventHandler<BookUpdatePolicyReaderBookDomainEvent>,
        IEventHandler<RemovedGenreForBookDomainEvent>
{
    private readonly ILogger<UpdateToElasticUpdatedBookDomainEventHandler> _logger = logger;
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries; 
    public async Task Handler(UpdatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await UpdateBookWhenChangedEvent(domainEvent.Book, cancellationToken);
    }

    public async Task Handler(AddedGenreForBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await UpdateBookWhenChangedEvent(domainEvent.Book, cancellationToken);
    }

    public async Task Handler(BookChangedReleaseTypeDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await UpdateBookWhenChangedEvent(domainEvent.Book, cancellationToken);
    }

    public async Task Handler(BookUpdatePolicyReaderBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await UpdateBookWhenChangedEvent(domainEvent.Book, cancellationToken);
    }

    public async Task Handler(RemovedGenreForBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await UpdateBookWhenChangedEvent(domainEvent.Book, cancellationToken);
    }

    private async Task UpdateBookWhenChangedEvent(Book book, CancellationToken cancellationToken)
    {
        var elasticForBook = _elasticFactory.GetInstance<BookElasticModel>();

        var bookElastic = await elasticForBook.GetAsync(book.Id.ToString());
        if ((bookElastic is null))
        {
            _logger.LogError("Can't not find book has id {Id} in elastic", book.Id);
            return;
        }
        var genreForBook = await _bookAuthoringQueries
            .FindGenresByIdsAsync(cancellationToken, book
                .Genres.Select(x => x.GenreId).ToArray());
        var bookElasticModel = book
            .ToElasticModel(genreForBook.ToList(), _identityProvider.UserName());
        _logger.LogInformation("Start write book for elastic when user has created book for {@BookId}", book.Id);
        await elasticForBook.UpdateAsync(bookElasticModel);
        _logger.LogInformation("Updated book has id {@Id}", book.Id);
    }
}
