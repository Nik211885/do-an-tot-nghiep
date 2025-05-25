using Application.Interfaces.CQRS;
using Core.Events.BookAuthoringContext;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.DomainEventHandler;

public class AddedGenreForBookDomainEventHandler(
    ILogger<AddedGenreForBookDomainEventHandler> logger,
    IBookRepository  bookRepository,
    IGenresRepository  genresRepository)
    : IEventHandler<AddedGenreForBookDomainEvent>
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

        var genres =
            await _genresRepository
                .FindActiveByIdsAsync(cancellationToken, 
                    domainEvent.BookGenres.Select(x=>x.GenreId).ToArray());
        Parallel.ForEach(genres, genre =>
        {
            genre.AddCoutForBook();
        });
        await _genresRepository.UpdateBulkAsync(genres, cancellationToken);

    }
}
