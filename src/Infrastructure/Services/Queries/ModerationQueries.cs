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
                        null
                        
                    ),
                    cancellationToken
                );
        return bookApproval;
    }
}
