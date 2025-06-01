using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.CQRS;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Core.Interfaces.Repositories.ModerationContext;
using Microsoft.Extensions.Logging;

namespace Application.BoundContext.ModerationContext.Command;

public record CreateBookApprovalCommand(Guid BookId, Guid ChapterId, string ChapterContent, Guid AuthorId)
    : IModerationCommand<BookApprovalViewModel>;

public class CreateBookApprovalCommandHandler(
    ILogger<CreateBookApprovalCommandHandler> logger,
    IBookApprovalRepository approvalRepository)
    : ICommandHandler<CreateBookApprovalCommand, BookApprovalViewModel>
{
    private readonly ILogger<CreateBookApprovalCommandHandler> _logger = logger;
    private readonly IBookApprovalRepository _approvalRepository = approvalRepository;
    public async Task<BookApprovalViewModel> Handle(CreateBookApprovalCommand request, CancellationToken cancellationToken)
    {
        var approval = BookApproval.Create(
            bookId: request.BookId,
            contentHash: request.ChapterContent,
            chapterId: request.ChapterId,
            authorId: request.AuthorId
        );
        _logger.LogInformation("Created book approval with id {Id}", approval.Id);
        var result = _approvalRepository.Add(approval);
        await _approvalRepository.UnitOfWork.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Created book approval success with id {Id}", result.Id);
        return result.ToViewModel();
    }
}
