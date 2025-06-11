using Application.BoundContext.ModerationContext.Queries;
using Application.BoundContext.ModerationContext.ViewModel;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class ModerationQueries(ModerationDbContext moderationDbContext) : IModerationQueries
{
    private readonly ModerationDbContext _moderationDbContext = moderationDbContext;

    public async Task<PaginationItem<BookApprovalViewModel>> GetBookApprovalWithPaginationByStatusAsync(
        BookApprovalStatus status, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var bookApproval = await _moderationDbContext.BookApprovals
                .Where(x => x.Status == status)
                .Include(x => x.Decision) // one-to-many, bảng riêng
                .OrderByDescending(x => x.SubmittedAt)
                .CreatePaginationAsync(
                    page,
                    approval => new BookApprovalViewModel(
                        approval.Id,
                        approval.BookId,
                        approval.ChapterId,
                        approval.AuthorId,
                        approval.SubmittedAt,
                        approval.ContentHash,
                        approval.Status,
                        approval.Version,
                        approval.Decision.Select(d => new ApprovalDecisionViewModel(
                            d.ModeratorId,
                            d.DecisionDateTime,
                            d.Note,
                            d.IsAutomated,
                            d.Status
                        )).ToList(),
                        new CopyrightChapterViewModel(
                            approval.CopyrightChapter.BookTitle,
                            approval.CopyrightChapter.ChapterTitle,
                            approval.CopyrightChapter.ChapterContent,
                            approval.CopyrightChapter.IsActive,
                            approval.CopyrightChapter.DateTimeCopyright,
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
}
