using Application.BoundContext.ModerationContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;
using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Application.BoundContext.ModerationContext.Queries;

public interface IModerationQueries : IApplicationQueryServicesExtension
{
    Task<PaginationItem<BookApprovalViewModel>> GetBookApprovalWithPaginationByStatusAsync
        (BookApprovalStatus status, PaginationRequest page, CancellationToken cancellationToken = default);
    Task<BookApprovalViewModel?> GetBookApprovalByIdAsync(Guid bookApprovalId, CancellationToken cancellationToken = default);
    Task<PaginationItem<ApprovalDecisionViewModel>> GetDecisionWithPaginationByApprovalIdAsync(Guid bookApprovalId, 
        PaginationRequest page, CancellationToken cancellationToken = default);
}
