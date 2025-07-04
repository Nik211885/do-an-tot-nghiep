using System.Security.Principal;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record RejectBookCommand(Guid Id, string? Note)
    : IModerationCommand<BookApprovalViewModel>;

public class RejectBookCommandHandler(
    ILogger<RejectBookCommandHandler> logger,
    IBookApprovalRepository bookApprovalRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<RejectBookCommand, BookApprovalViewModel>
{
    private readonly ILogger<RejectBookCommandHandler> _logger = logger;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<BookApprovalViewModel> Handle(RejectBookCommand request, CancellationToken cancellationToken)
    {
        var bookApproval = await _bookApprovalRepository.FindByIdAsync(request.Id, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Sách cần kiểm duyệt");
        bookApproval.Reject(_identityProvider.UserIdentity(), request.Note, false);
        _bookApprovalRepository.Update(bookApproval);
        _logger.LogInformation("Star reject book has reason {@Id}", request.Id);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return bookApproval.ToViewModel();
    }
}


