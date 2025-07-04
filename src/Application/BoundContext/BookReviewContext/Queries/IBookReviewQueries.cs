using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Interfaces.Query;
using Application.Models;

namespace Application.BoundContext.BookReviewContext.Queries;

public interface IBookReviewQueries : IApplicationQueryServicesExtension
{
    Task<IEnumerable<BookReviewViewModel>> GetBookReviewByBookIdsAsync(CancellationToken cancellationToken = default,params Guid[] ids);
    Task<BookReviewViewModel?> GetBookReviewByBookId(Guid bookId, CancellationToken cancellationToken = default);

    Task<PaginationItem<CommentViewModel>?> GetCommentWithPaginationByBookIdAsync(Guid bookId, PaginationRequest page,
        CancellationToken cancellationToken = default);
    
    Task<PaginationItem<RatingViewModel>?> GetRatingWithPaginationByBookIdAsync(Guid bookId, 
        PaginationRequest page,CancellationToken cancellationToken = default);

    Task<PaginationItem<CommentViewModel>> GetCommentWithPaginationByUserIdAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken =default);

    Task<PaginationItem<RatingViewModel>> GetRatingWithPaginationByUserIdAsync(Guid userId, PaginationRequest page,
        CancellationToken cancellationToken = default);
    Task<RatingViewModel?> GetRatingByUSerIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RatingViewModel>> GetRatingByBookIdsForUserAsync(Guid userId, Guid[] bookIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CommentViewModel>> GetAllCommentByUserIdAndBookIdAsync(Guid userId, Guid bookId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BookReviewViewModel>> GetBookReviewHasTopViewBookAsync(int top, CancellationToken cancellationToken = default);
    Task<PaginationItem<CommentViewModel>> GetCommentReplyWithPaginationAsync(Guid commentReplyId, PaginationRequest page, CancellationToken cancellationToken = default);
}
