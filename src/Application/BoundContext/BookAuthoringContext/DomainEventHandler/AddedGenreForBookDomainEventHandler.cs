using Application.Interfaces.CQRS;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Events.BookAuthoringContext;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class AddedGenreForBookDomainEventHandler(
    ILogger<AddedGenreForBookDomainEventHandler> logger,
    IBookRepository  bookRepository,
    IGenresRepository  genresRepository)
    : IEventHandler<AddedGenreForBookDomainEvent>,
        IEventHandler<CreatedBookDomainEvent>
{
    private readonly ILogger<AddedGenreForBookDomainEventHandler> _logger = logger;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IGenresRepository _genresRepository = genresRepository;
    public async Task Handler(AddedGenreForBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.FindByIdAsync(domainEvent.BookId, cancellationToken);
        if (book is null)
        {
            _logger.LogError("Not find book in event store {domainEvent}", domainEvent);
            return;
        }

        await CoutForGenreWhenAddGenreForBookAsync(domainEvent.Book, cancellationToken);
    }

    public async Task Handler(CreatedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await CoutForGenreWhenAddGenreForBookAsync(domainEvent.Book, cancellationToken);
    }

    private async Task CoutForGenreWhenAddGenreForBookAsync(Book book, CancellationToken cancellationToken)
    {
        var genres =
            await _genresRepository
                .FindActiveByIdsAsync(cancellationToken, 
                    false,book.Genres.Select(x=>x.GenreId).ToArray());
        Parallel.ForEach(genres, genre =>
        {
            genre.AddCoutForBook();
        });
        await _genresRepository.UpdateBulkAsync(genres, cancellationToken);
    }
}
