using Application.BoundContext.ModerationContext.IntegrationEvent.Event;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.EventBus;
using Application.Interfaces.IdentityProvider;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record ApprovalChapterCommand(Guid ChapterApprovalId, string Note)
    : IModerationCommand<ChapterApprovalViewModel>;

public class ApprovalChapterCommandHandler(
    ILogger<ApprovalChapterCommandHandler> logger,
    IChapterApprovalRepository chapterApprovalRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<ApprovalChapterCommand, ChapterApprovalViewModel>
{
    private readonly IIdentityProvider _identityProvider = identityProvider;
    private readonly ILogger<ApprovalChapterCommandHandler> _logger = logger;
    private readonly IChapterApprovalRepository _chapterApprovalRepository = chapterApprovalRepository;
    public async Task<ChapterApprovalViewModel> Handle(ApprovalChapterCommand request, CancellationToken cancellationToken)
    {
        ChapterApproval? chapterApproval =  await _chapterApprovalRepository.FindByIdAsync(request.ChapterApprovalId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(chapterApproval, "Chuong");
        chapterApproval.Approved(_identityProvider.UserIdentity(), request.Note);
        _logger.LogInformation("Approval chapter for id {id}", request.ChapterApprovalId);
        _chapterApprovalRepository.Update(chapterApproval);
        await _chapterApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return chapterApproval.ToViewModel();
    }
}
