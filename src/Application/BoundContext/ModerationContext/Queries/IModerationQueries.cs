using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;

namespace Application.BoundContext.ModerationContext.Queries;

public interface IModerationQueries : IApplicationQueryServicesExtension
{
    Task<PaginationItem<ApprovalDecisionViewModel>> GetDecisionForChapterApprovalAsync(Guid chapterApprovalId, PaginationRequest page , CancellationToken cancellationToken =default);
    Task<PaginationItem<ChapterApprovalViewModel>> GetChapterNeedModerationWithPaginationAsync(PaginationRequest page, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ChapterStoreViewModel>> GetChaptersForBookAsync(Guid bookId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BookApprovalViewModel>>  GetBookApprovalByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task<ChapterApprovalViewModel?> GetChapterApprovalDetailAsync(Guid chapterApprovalId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<ChapterApprovalViewModel>> GetChapterApprovalForBookId(Guid bookApprovalId, CancellationToken cancellationToken = default);
    Task<PaginationItem<BookApprovalViewModel>> GetBookApprovalWithPaginationAsync(string? title, PaginationRequest page, CancellationToken cancellationToken = default);
    ValueTask<string> GetContentForChapterAsync(Guid bookId, string chapterSlug, CancellationToken cancellationToken);
}
