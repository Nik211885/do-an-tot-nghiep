using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record ActiveBookApprovalCommand(Guid BookId)
    : IModerationCommand<BookApprovalViewModel>;

public class ActiveBookApprovalCommandHandler(
    ILogger<ActiveBookApprovalCommandHandler> logger,
    IBookApprovalRepository bookApprovalRepository)
    : ICommandHandler<ActiveBookApprovalCommand, BookApprovalViewModel>
{
    private readonly ILogger<ActiveBookApprovalCommandHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task<BookApprovalViewModel> Handle(ActiveBookApprovalCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start active book has id {@ID}", request.BookId);
        BookApproval? bookApproval = await _bookApprovalRepository.FindByBookIdAsync(request.BookId, cancellationToken);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(bookApproval, "Sach");
        bookApproval.Active();
        _logger.LogInformation("Finished active book with id {@ID}", request.BookId);
        _bookApprovalRepository.Update(bookApproval);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return bookApproval.ToViewModel();
    }
}
