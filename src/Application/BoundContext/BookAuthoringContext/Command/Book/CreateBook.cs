using Application.BoundContext.BookAuthoringContext.Message;
using Application.BoundContext.BookAuthoringContext.ViewModel;
using Application.Exceptions;
using Application.Helper;
using Application.Interfaces.Cache;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.BookAuthoringContext.BookAggregate;
using Core.Interfaces.Repositories.BookAuthoringContext;
using Microsoft.Extensions.Logging;
using BookCore = Core.BoundContext.BookAuthoringContext.BookAggregate.Book;

namespace Application.BoundContext.BookAuthoringContext.Command.Book;

public record CreateBookCommand(
    string Title,
    string? AvatarUrl,
    string? Description,    
    BookPolicy ReaderBookPolicy,
    decimal? ReaderBookPolicyPrice,
    BookReleaseType BookReleaseType,
    IReadOnlyCollection<string> TagsName,
    IReadOnlyCollection<Guid> GenreIds) : IBookAuthoringCommand<BookViewModel>;

public class CreateBookCommandHandler(IBookRepository bookRepository,
    IGenresRepository genresRepository,
    IIdentityProvider identityProvider,
    ILogger<CreateBookCommandHandler> logger) 
    : ICommandHandler<CreateBookCommand, BookViewModel>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly IGenresRepository _genresRepository = genresRepository;
    private readonly ILogger<CreateBookCommandHandler> _logger = logger;
    public async Task<BookViewModel> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        //
        var genresByIds = await _genresRepository
            .FindActiveByIdsAsync(cancellationToken, true,request.GenreIds.ToArray());
        // You have compare with count genres user  request with genres has find in repository
        // It sure user don't miss genres active when user create new book
        if (genresByIds.Count != request.GenreIds.Count)
        {
            ThrowHelper.ThrowIfBadRequest(BookValidationMessages.HasGenreNotExit);
        }
        var book = BookCore.Create(
            createdUserid: _identityProvider.UserIdentity(),
            title: request.Title,
            avatarUrl: request.AvatarUrl,
            description: request.Description,
            readerBookPolicy: request.ReaderBookPolicy,
            priceReaderBookPolicy: request.ReaderBookPolicyPrice,
            bookReleaseType: request.BookReleaseType,
            tagNames: request.TagsName,
            genreIds: genresByIds.Select(x=>x.Id).ToList(),
            slug: request.Title.CreateSlug()
        );
        _logger.LogInformation("Create book {book}", book);
        _bookRepository.Create(book);
        await _bookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Created book success {book}", book);
        return book.MapToViewModel();
    }
}
