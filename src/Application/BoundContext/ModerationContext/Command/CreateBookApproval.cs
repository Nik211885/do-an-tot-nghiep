using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record CreateBookApprovalCommand(Guid BookId, string BookTitle, Guid AuthorId)
    : IModerationCommand<BookApprovalViewModel>;

public class CreateBookApprovalCommandHandler(
    IBookApprovalRepository bookApprovalRepository,
    ILogger<CreateBookApprovalCommandHandler> logger)
    : ICommandHandler<CreateBookApprovalCommand, BookApprovalViewModel>
{
    private readonly ILogger<CreateBookApprovalCommandHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task<BookApprovalViewModel> Handle(CreateBookApprovalCommand request, CancellationToken cancellationToken)
    {
        var bookApproval = BookApproval.Create(request.BookId, request.AuthorId, request.BookTitle);
        _bookApprovalRepository.Create(bookApproval);
        _logger.LogInformation("Create book approval {bookApproval}", bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return bookApproval.ToViewModel();
    }
}
