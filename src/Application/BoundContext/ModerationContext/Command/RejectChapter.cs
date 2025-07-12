using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Application.Interfaces.IdentityProvider;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record RejectChapterCommand(Guid ChapterApprovalId, string Note)
    : IModerationCommand<ChapterApprovalViewModel>;


public class RejectChapterCommandHandler(
    ILogger<RejectChapterCommandHandler> logger,
    IChapterApprovalRepository chapterApprovalRepository,
    IIdentityProvider identityProvider)
    : ICommandHandler<RejectChapterCommand, ChapterApprovalViewModel>
{
    private readonly ILogger<RejectChapterCommandHandler> _logger = logger;
    private readonly IChapterApprovalRepository _chapterApprovalRepository = chapterApprovalRepository;
    private readonly IIdentityProvider _identityProvider = identityProvider;
    public async Task<ChapterApprovalViewModel> Handle(RejectChapterCommand request, CancellationToken cancellationToken)
    {
        var chapterApproval = await _chapterApprovalRepository.FindByChapterIdAsync(request.ChapterApprovalId);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(chapterApproval, "Chuong");
        chapterApproval.Rejected(_identityProvider.UserIdentity(), request.Note);
        _logger.LogInformation("Rejected approval has id {Id}", request.ChapterApprovalId);
        _chapterApprovalRepository.Update(chapterApproval);
        await _chapterApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return chapterApproval.ToViewModel();
    }
}
