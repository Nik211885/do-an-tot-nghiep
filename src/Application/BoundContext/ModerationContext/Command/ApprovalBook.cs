using Application.BoundContext.ModerationContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record ApprovalBookCommand(Guid Id, string? Note)
    : IModerationCommand<BookApprovalViewModel>;

public class ApprovalBookCommandHandler(
    ILogger<ApprovalBookCommand> logger,
    IBookApprovalRepository approvalRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<ApprovalBookCommand, BookApprovalViewModel>
{
    private readonly ILogger<ApprovalBookCommand> _logger = logger;
    private readonly IBookApprovalRepository _approvalRepository = approvalRepository;
    private readonly  IIdentityProvider _identityProvider = identityProvider;
    public async Task<BookApprovalViewModel> Handle(ApprovalBookCommand request, CancellationToken cancellationToken)
    {
        var bookApproval = await _approvalRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Tác phẩm cần duyệt");
        _logger.LogInformation("Satrt approve book has Reason {@Reason}", request.Id);
        bookApproval.Approve(_identityProvider.UserIdentity(), request.Note, false);
        _approvalRepository.Update(bookApproval);
        await _approvalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return bookApproval.ToViewModel();
    }
}
