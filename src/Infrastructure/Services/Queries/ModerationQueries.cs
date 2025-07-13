using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Exceptions;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class ModerationQueries(ModerationDbContext 
    moderationDbContext) : IModerationQueries
{
    private readonly ModerationDbContext _moderationDbContext = moderationDbContext;

    public async Task<PaginationItem<ApprovalDecisionViewModel>> GetDecisionForChapterApprovalAsync(Guid chapterApprovalId,PaginationRequest page, CancellationToken cancellationToken =default)
    {
        IQueryable<ApprovalDecision> queryable = _moderationDbContext.ApprovalDecisions
            .AsNoTracking()
            .Where(x => x.ChapterApprovalId == chapterApprovalId)
            .OrderByDescending(x => x.DecisionDateTime);
        PaginationItem<ApprovalDecisionViewModel> paginationItem
            = await queryable.CreatePaginationAsync<ApprovalDecision,ApprovalDecisionViewModel>(page,
                ApprovalDecisionMappingExtension.ToViewModel(),
                cancellationToken
                );
        return paginationItem;
    }

    public async Task<PaginationItem<ChapterApprovalViewModel>> GetChapterNeedModerationWithPaginationAsync(PaginationRequest page, CancellationToken cancellationToken = default)
    {
        IQueryable<ChapterApproval> queryable
            = _moderationDbContext.ChapterApprovals
                .AsNoTracking()
                .Where(x => x.Status == ChapterApprovalStatus.Pending)
                .OrderBy(x=>x.SubmittedAt);
        PaginationItem<ChapterApprovalViewModel> paginationItem
            = await queryable.CreatePaginationAsync<ChapterApproval, ChapterApprovalViewModel>(
                page,
                ChapterApprovalMappingExtension.ToViewModel(),
                cancellationToken
            );
        return paginationItem;
    }

    public async Task<IReadOnlyCollection<ChapterStoreViewModel>> GetChaptersForBookAsync(Guid bookId, CancellationToken cancellationToken= default)
    {
        IQueryable<BookApproval> bookApprovalQuery = _moderationDbContext.BookApprovals
            .AsNoTracking()
            .Where(x => x.IsActive
                && x.BookId == bookId);
        BookApproval? bookApproval
            = await bookApprovalQuery.FirstOrDefaultAsync(cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Sach");
        IQueryable<ChapterApproval> chapterApprovalQuery = _moderationDbContext.ChapterApprovals
            .AsNoTracking()
            .Where(x=>x.BookApprovalId == bookApproval.Id
                &&  x.Status ==  ChapterApprovalStatus.Approved
                && x.CopyrightChapter != null)
            .OrderBy(x=>x.ChapterNumber);
        IEnumerable<ChapterApproval> chapterApprovals
            = await chapterApprovalQuery.ToListAsync(cancellationToken);
        IReadOnlyCollection<ChapterStoreViewModel> chapterApprovalViewModels
            = chapterApprovals.Select(x=> new ChapterStoreViewModel(
                x.CopyrightChapter!.ChapterTitle,
                x.CopyrightChapter.ChapterSlug,
                x.CopyrightChapter.ChapterNumber
                )).ToList();
        return chapterApprovalViewModels;
    }

    public async Task<IReadOnlyCollection<BookApprovalViewModel>> GetBookApprovalByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        IQueryable<BookApproval> queryable = _moderationDbContext.BookApprovals
            .AsNoTracking()
            .Where(x => ids.Contains(x.Id));
        var bookApproval = await queryable.ToListAsync(cancellationToken);
        return bookApproval.ToViewModel();
    }

    public async Task<ChapterApprovalViewModel?> GetChapterApprovalDetailAsync(Guid chapterApprovalId, CancellationToken cancellationToken = default)
    {
        IQueryable<ChapterApproval> queryable
            = _moderationDbContext.ChapterApprovals
                .AsNoTracking()
                .Where(x => x.Id == chapterApprovalId)
                .Include(x => x.Decision);
        ChapterApproval? chapterApproval = await queryable.FirstOrDefaultAsync(cancellationToken);
        return chapterApproval?.ToViewModel();
    }

    public async Task<IReadOnlyCollection<ChapterApprovalViewModel>> GetChapterApprovalForBookId(Guid bookApprovalId, CancellationToken cancellationToken = default)
    {
        IQueryable<ChapterApproval> queryable
            = _moderationDbContext.ChapterApprovals
                .AsNoTracking()
                .Where(x => x.BookApprovalId == bookApprovalId)
                .OrderBy(x => x.SubmittedAt)
                .Include(x => x.Decision);
        var result = await 
            queryable.Select(ChapterApprovalMappingExtension.ToViewModel())
                .ToListAsync(cancellationToken);
        return result;
    }

    public async Task<PaginationItem<BookApprovalViewModel>> GetBookApprovalWithPaginationAsync(string? title, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        IQueryable<BookApproval> queryable = _moderationDbContext
            .BookApprovals.AsNoTracking()
            .Where(x => 
                (title == null || x.BookTitle.ToLower().Contains(title.ToLower()))
                && x.ChapterCount > 0
            )
            .OrderByDescending(x => x.SubmittedOn);
        var pagination = await queryable.CreatePaginationAsync<BookApproval, BookApprovalViewModel>(
            page,
            BookApprovalViewModelMappingExtension.ToViewModel(),
            cancellationToken
        );
        return pagination;
    }
    public async ValueTask<string> GetContentForChapterAsync(Guid bookId, string chapterSlug, CancellationToken cancellationToken)
    {
        IQueryable<BookApproval> bookApprovalQuery =  _moderationDbContext.BookApprovals
            .AsNoTracking()
            .Where(x=>x.BookId == bookId);
        BookApproval? bookApproval = await bookApprovalQuery
            .FirstOrDefaultAsync(cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(bookApproval, "Sach");
        if (!bookApproval.IsActive)
        {
            ThrowHelper.ThrowNotFound("Sach");
        }
        IQueryable<ChapterApproval> chapterApprovalQuery = _moderationDbContext.ChapterApprovals
            .AsNoTracking()
            .Include(x=>x.CopyrightChapter)
            .Where(x=>x.CopyrightChapter != null && x.CopyrightChapter.ChapterSlug == chapterSlug);
        ChapterApproval? chapterApproval = await chapterApprovalQuery
            .FirstOrDefaultAsync(cancellationToken);
        ThrowHelper.ThrowNotFoundWhenItemIsNull(chapterApproval, "Chuong");
        return chapterApproval.CopyrightChapter!.ChapterContent;
    }
}
