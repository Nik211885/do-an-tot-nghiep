using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Exception;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record CreateChapterApprovalCommand(Guid BookId,
    Guid ChapterId,
        string ChapterContent, 
        string ChapterTitle,
        int ChapterNumber,string ChapterSlug)
    : IModerationCommand<ChapterApprovalViewModel>;

public class CreateChapterApprovalCommandHandler(
    ILogger<CreateChapterApprovalCommandHandler> logger,
    IChapterApprovalRepository chapterApprovalRepository,
    IBookApprovalRepository bookApprovalRepository)
    : ICommandHandler<CreateChapterApprovalCommand, ChapterApprovalViewModel>
{
    private readonly ILogger<CreateChapterApprovalCommandHandler> _logger = logger;
    private readonly IChapterApprovalRepository _chapterApprovalRepository = chapterApprovalRepository;
    private readonly IBookApprovalRepository _bookApprovalRepository = bookApprovalRepository;
    public async Task<ChapterApprovalViewModel> Handle(CreateChapterApprovalCommand request, CancellationToken cancellationToken)
    {
        var bookApproval = await _bookApprovalRepository.FindByBookIdAsync(request.BookId, cancellationToken);
        ThrowHelper.ThrowBadRequestWhenArgumentIsNull(bookApproval, "Sach");
        var chapterApprovalExits = await _chapterApprovalRepository.FindByChapterIdAsync(request.ChapterId);
        if (chapterApprovalExits is null)
        {
            _logger.LogInformation("Dont find chapter approval exits and created new for chapter id {ChapterId}", request.ChapterId);
            var chapterApproval = ChapterApproval.Create(bookApproval.Id, request.ChapterId, request.ChapterContent,
                request.ChapterTitle, request.ChapterNumber, request.ChapterSlug);
            chapterApprovalExits = _chapterApprovalRepository.Create(chapterApproval);
        }
        else
        {
            _logger.LogInformation("Update chapter id {ChapterId}", request.ChapterId);
            chapterApprovalExits.OpenApproval();
            _chapterApprovalRepository.Update(chapterApprovalExits);
        }
        await _chapterApprovalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        return chapterApprovalExits.ToViewModel();
    }
}

