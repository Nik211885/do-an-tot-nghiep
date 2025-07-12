using Application.BoundContext.ModerationContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record UnActiveBookApprovalCommand(Guid BookId)
    : IModerationCommand<BookApprovalViewModel>;

public class UnActiveBookApprovalCommandHandler(
    ILogger<UnActiveBookApprovalCommandHandler> logger,
    IBookApprovalRepository bookApprovalRepository)
    : ICommandHandler<UnActiveBookApprovalCommand, BookApprovalViewModel>
{
    private readonly ILogger<UnActiveBookApprovalCommandHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task<BookApprovalViewModel> Handle(UnActiveBookApprovalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start un active book has id {@ID}", request.BookId);
        BookApproval? bookApproval = await _bookApprovalRepository.FindByBookIdAsync(request.BookId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Sach");
        bookApproval.UnActive();
        _logger.LogInformation("Finished un active book with id {@ID}", request.BookId);
        _bookApprovalRepository.Update(bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return bookApproval.ToViewModel();
    }
}
