using Application.BoundContext.ModerationContext.ViewModel;
using Application.Exceptions;
using Application.Interfaces.CQRS;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record CreateSignatureCommand
(Guid BookApprovalId, string SignatureValue, string SignatureAlgorithm)
    : IModerationCommand<BookApprovalViewModel>;

public class CreateSignatureCommandHandler(
    IBookApprovalRepository approvalRepository,
    ILogger<CreateSignatureCommandHandler> logger)
    : ICommandHandler<CreateSignatureCommand, BookApprovalViewModel>
{
    private readonly IBookApprovalRepository _approvalRepository = approvalRepository;
    private readonly ILogger<CreateSignatureCommandHandler> _logger = logger;
    public async Task<BookApprovalViewModel> Handle(CreateSignatureCommand request, CancellationToken cancellationToken)
    {
        var approval = await _approvalRepository.FindByIdAsync(request.BookApprovalId, cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(approval, "CreateSignatureCommand");
        approval.AddSignature(request.SignatureValue, request.SignatureAlgorithm);
        _approvalRepository.Update(approval);
        return approval.ToViewModel();
    }
}


