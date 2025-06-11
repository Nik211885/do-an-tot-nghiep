using System.Security.Claims;
using Application.BoundContext.BookAuthoringContext.Queries;
using Application.Interfaces.CQRS;
using Application.Interfaces.Elastic;
using Application.Interfaces.IdentityProvider;
using Application.Models;
using Core.Events.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class WriteToElasticCreatedBookDomainEventHandler(
    ILogger<WriteToElasticCreatedBookDomainEventHandler> logger,
    IElasticFactory elasticFactory,
    IBookAuthoringQueries bookAuthoringQueries,
    IIdentityProvider identityProvider)
    : IEventHandler<CreatedBookDomainEvent>
{
    private readonly ILogger<WriteToElasticCreatedBookDomainEventHandler> _logger = logger;
    private readonly IElasticFactory _elasticFactory = elasticFactory;
    private readonly IBookAuthoringQueries _bookAuthoringQueries = bookAuthoringQueries;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task Handler(CreatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var elasticForBook = _elasticFactory.GetInstance<BookElasticModel>();
        var genreForBook = await _bookAuthoringQueries
            .FindGenresByIdsAsync(cancellationToken, domainEvent.Book
                .Genres.Select(x => x.GenreId).ToArray());
        var bookElasticModel = domainEvent.Book
            .ToElasticModel(genreForBook.ToList(), _identityProvider.UserName());
        _logger.LogInformation("Start write book for elastic when user has created book for {@BookId}", domainEvent.Book.Id);
        await elasticForBook.AddAsync(bookElasticModel);
    }
}
