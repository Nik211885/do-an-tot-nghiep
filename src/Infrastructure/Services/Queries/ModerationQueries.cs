using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class ModerationQueries(ModerationDbContext moderationDbContext)
    : IModerationQueries
{
    private readonly ModerationDbContext _moderationDbContext = moderationDbContext;

    public async Task<PaginationItem<BookApprovalViewModel>> GetBookApprovalWithPaginationByStatusAsync(
        BookApprovalStatus status, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var bookApproval = await _moderationDbContext.BookApprovals
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.SubmittedAt)
                .CreatePaginationAsync(
                    page,
                    approval => new BookApprovalViewModel(
                        approval.Id,
                        approval.BookId,
                        approval.ChapterId,
                        approval.AuthorId,
                        approval.SubmittedAt,
                        approval.ChapterTitle,
                        approval.ChapterNumber,
                        approval.ChapterSlug,
                        approval.BookTitle,
                        approval.ChapterContent,
                        approval.Status,
                        null,
                        approval.CopyrightChapter == null
                            ? null
                            : new CopyrightChapterViewModel(
                                approval.CopyrightChapter.BookTitle,
                                approval.CopyrightChapter.ChapterTitle,
                                approval.CopyrightChapter.ChapterContent,
                                approval.CopyrightChapter.DateTimeCopyright,
                                approval.CopyrightChapter.ChapterSlug,
                                approval.CopyrightChapter.ChapterNumber,
                                approval.CopyrightChapter.DigitalSignature == null
                                    ? null
                                    : new DigitalSignatureViewModel(
                                        approval.CopyrightChapter.DigitalSignature.SignatureValue,
                                        approval.CopyrightChapter.DigitalSignature.SignatureAlgorithm,
                                        approval.CopyrightChapter.DigitalSignature.SigningDateTime
                                    )
                            )
                    ),
                    cancellationToken
                );
        return bookApproval;
    }

    public async Task<BookApprovalViewModel?> GetBookApprovalByIdAsync(Guid bookApprovalId, CancellationToken cancellationToken = default)
    {
        var bookApproval = await _moderationDbContext.BookApprovals
            .AsNoTracking()
            .Where(x => x.Id == bookApprovalId)
            .FirstOrDefaultAsync(cancellationToken);
        return bookApproval?.ToViewModel();    
    }

    public async Task<PaginationItem<ApprovalDecisionViewModel>> GetDecisionWithPaginationByApprovalIdAsync(Guid bookApprovalId, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var decision = await
            _moderationDbContext.BookApprovals.AsNoTracking()
                .Where(x => x.Id == bookApprovalId)
                .Include(x => x.Decision)
                .SelectMany(x => x.Decision)
                .CreatePaginationAsync(page, m =>
                        new ApprovalDecisionViewModel(
                            m.ModeratorId,m.DecisionDateTime,
                            m.Note,
                            m.IsAutomated,
                            m.Status
                            )
                , cancellationToken);
        return decision;
    }

    public async Task<IReadOnlyCollection<ChapterStoreViewModel>> GetAllChapterForBookIdAsync(
        Guid bookId, CancellationToken cancellationToken = default)
    {
        var query = _moderationDbContext.BookApprovals
            .Where(x => x.BookId == bookId 
                        && x.CopyrightChapter != null)
            .OrderBy(x=>x.CopyrightChapter!.ChapterNumber)
            .Select(y => new ChapterStoreViewModel(
                    y.CopyrightChapter!.ChapterTitle,
                    y.CopyrightChapter.ChapterSlug,
                    y.CopyrightChapter.ChapterNumber
                ));
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<string?> GetContentForChapterAsync(string chapterSlug,
        CancellationToken cancellationToken = default)
    {
        var query = _moderationDbContext.BookApprovals
            .Where(x => x.CopyrightChapter != null
                        && x.CopyrightChapter.ChapterSlug == chapterSlug)
            .Select(y => y.CopyrightChapter!.ChapterContent);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
}
