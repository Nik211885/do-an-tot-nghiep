using Application.BoundContext.BookReviewContext.Queries;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Validator;
using Core.BoundContext.BookReviewContext.ReaderBookAggregate;
using Core.Interfaces.Repositories.BookReviewContext;

namespace Application.BoundContext.BookReviewContext.Command.Reader;

public record CreateReaderCommand(Guid BookId, Guid CreatedUserId)
    : IBookReviewCommand<Guid>;

public class CreateReaderCommandHandler(IReaderBookRepository readerBookRepository, 
    IIdentityProvider identityProvider, 
    IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview> validationServices)
    : ICommandHandler<CreateReaderCommand, Guid>
{
    private readonly IReaderBookRepository _readerBookRepository = readerBookRepository;

    private readonly IValidationServices<Core.BoundContext.BookReviewContext.BookReviewAggregate.BookReview>
        _validationServices = validationServices;
    public async Task<Guid> Handle(CreateReaderCommand request, CancellationToken cancellationToken)
    {
        var bookReview = await _validationServices.AnyAsync(x=>x.BookId == request.BookId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookReview, "Sách");
        var reader = ReaderBook.Create(bookReview.Id,  request.CreatedUserId);
        _readerBookRepository.Create(reader);
        await _readerBookRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return reader.Id;
    }
}
