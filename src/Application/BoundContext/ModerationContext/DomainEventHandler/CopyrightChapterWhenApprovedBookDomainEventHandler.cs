using Application.Interfaces.CQRS;
using Application.Interfaces.Signature;
using Core.Events.ModerationContext;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.DomainEventHandler;

public class DigitalSignatureChapterWhenApprovedBookDomainEventHandler(
    ILogger<DigitalSignatureChapterWhenApprovedBookDomainEventHandler> logger,
    IDigitalSignatureService digitalSignature,
    IBookApprovalRepository bookApprovalRepository)
    : IEventHandler<ApprovedBookDomainEvent>
{
    private readonly ILogger<DigitalSignatureChapterWhenApprovedBookDomainEventHandler> _logger = logger;
    private readonly IDigitalSignatureService _digitalSignature = digitalSignature;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public Task Handler(ApprovedBookDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
