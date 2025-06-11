/*
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Application.Interfaces.Signature;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;
//  When book has approval create chapter for digital sginature
public class DigitalSignatureChapterWhenApprovedBookDomainEventHandler(
    ILogger<DigitalSignatureChapterWhenApprovedBookDomainEventHandler> logger,
    IDigitalSignatureService digitalSignature,
    IIdentityProviderServices identityProviderServices,
    IBookApprovalRepository bookApprovalRepository)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly IIdentityProviderServices _identityProviderServices = identityProviderServices;
    private readonly ILogger<DigitalSignatureChapterWhenApprovedBookDomainEventHandler> _logger = logger;
    private readonly IDigitalSignatureService _digitalSignature = digitalSignature;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var approval = await _bookApprovalRepository.FindByIdAsync(domainEvent.Approval.Id, cancellationToken);
        if (approval is null)
        {
            return;
        }
        var userInfo = await _identityProviderServices.GetUserInfoAsync(approval.AuthorId.ToString());
        approval.CopyrightChapter.AddSignature(signatureValue.SignatureAlgorithm, signatureValue.SignatureValue);
        _bookApprovalRepository.Update(approval);
        _logger.LogInformation("Add signature for book has id: {BookId}", approval.BookId);
        await _bookApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
    }
}
*/
