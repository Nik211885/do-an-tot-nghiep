using Application.Helper;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.BookAuthoringContext.Command;

public record CreateBookAuthoringCommand(
    string Title,
    string? AvatarUrl,
    string? Description,    
    int VersionNumber,
    BookPolicy ReaderBookPolicy,
    decimal? ReaderBookPolicyPrice,
    BookReleaseType BookReleaseType,
    IReadOnlyCollection<string> TagsName,
    bool Visibility,
    IReadOnlyCollection<Guid> GenreIds) : IBookAuthoringCommand<Book>;
/// <summary>
/// 
/// </summary>
/// <param name="bookRepository"></param>
/// <param name="genresRepository"></param>
public class CreateBookCommandHandler(IBookRepository bookRepository,
    IGenresRepository genresRepository,
    IIdentityProvider identityProvider,
    ILogger<CreateBookCommandHandler> logger) 
    : ICommandHandler<CreateBookAuthoringCommand, Book>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IGenresRepository _genresRepository = genresRepository;
    private readonly ILogger<CreateBookCommandHandler> _logger = logger;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<Book> Handle(CreateBookAuthoringCommand request, CancellationToken cancellationToken)
    {
        var genresByIds = await _genresRepository
            .GetGenresActiveByIdsAsync(request.GenreIds.ToArray());
        // You have compare with count genres user  request with genres has find in repository
        // It sure user don't miss genres active when user create new book
        var book = Book.Create(
            createdUserid: _identityProvider.UserIdentity(),
            title: request.Title,
            avatarUrl: request.AvatarUrl,
            description: request.Description,
            versionNumber: request.VersionNumber,
            readerBookPolicy: request.ReaderBookPolicy,
            priceReaderBookPolicy: request.ReaderBookPolicyPrice,
            bookReleaseType: request.BookReleaseType,
            tagNames: request.TagsName,
            visibility: request.Visibility,
            genres: genresByIds,
            slug: request.Title.CreateSlug()
        );
        _bookRepository.AddEntity(book);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return book;
    }
}
