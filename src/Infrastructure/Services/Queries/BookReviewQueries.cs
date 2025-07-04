using Application.BoundContext.BookReviewContext.Queries;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Models;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class BookReviewQueries(BookReviewDbContext bookReviewDbContext) 
    : IBookReviewQueries
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
    public async Task<IEnumerable<BookReviewViewModel>> GetBookReviewByBookIdsAsync(CancellationToken cancellationToken = default, params Guid[] ids)
    {
        var query =
            _bookReviewDbContext.BookReviews
                .AsNoTracking()
                .Where(x => ids.Contains(x.BookId))
                .Select(x=>new BookReviewViewModel(
                    x.Id,
                    x.BookId,
                        x.ViewCount,
                        x.CommentCount,
                    x.RatingCount,
                    x.TotalRating,
                    x.CreatedAt,
                    x.LastUpdated
                    ));
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<BookReviewViewModel?> GetBookReviewByBookId(Guid bookId, CancellationToken cancellationToken = default)
    {
        var result = await _bookReviewDbContext.BookReviews
            .AsNoTracking()
            .Where(bookReview => bookReview.BookId == bookId)
            .OrderByDescending(x=>x.LastUpdated)
            .FirstOrDefaultAsync(cancellationToken);
        return result?.MapToViewModel();
    }

    public async Task<PaginationItem<CommentViewModel>?> GetCommentWithPaginationByBookIdAsync(Guid bookId, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var bookReviews = await GetBookReviewByBookId(bookId, cancellationToken);
        if (bookReviews == null)
        {
            return null;
        }
        var commentWithPagination = await _bookReviewDbContext.Comments
            .AsNoTracking()
            .Where(x=>x.BookReviewId == bookReviews.Id 
                      && x.ParentCommentId == null)
            .OrderByDescending(x=>x.DatetimeCommented)
            .CreatePaginationAsync(page, comment => new CommentViewModel(
                comment.Id,
                comment.BookReviewId,
                comment.ReviewerId,
                comment.Content,
                comment.DatetimeCommented,
                comment.ParentCommentId,
                comment.ReplyCount
                ), cancellationToken);
        return commentWithPagination;
    }

    public async Task<PaginationItem<RatingViewModel>?> GetRatingWithPaginationByBookIdAsync(Guid bookId, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var bookReviews = await GetBookReviewByBookId(bookId, cancellationToken);
        if (bookReviews == null)
        {
            return null;
        }
        var ratingWithPagination = await _bookReviewDbContext.Ratings
            .AsNoTracking()
            .Where(x=>x.BookReviewId == bookReviews.Id)
            .OrderByDescending(x=>x.LastUpdated)
            .CreatePaginationAsync(page, rating => new RatingViewModel(
                rating.Id,
                rating.BookReviewId,
                rating.ReviewerId,
                rating.Star.Star,
                rating.DateTimeSubmitted,
                rating.LastUpdated,
                bookId
            ), cancellationToken);
        return ratingWithPagination;
    }

    public async Task<PaginationItem<CommentViewModel>> GetCommentWithPaginationByUserIdAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken)
    {
        var comments = await _bookReviewDbContext.Comments
            .AsNoTracking()
            .Where(x=>x.ReviewerId == userId)
            .OrderByDescending(x=>x.DatetimeCommented)
            .CreatePaginationAsync(page, comment=>new CommentViewModel(
                comment.Id,
                comment.BookReviewId,
                comment.ReviewerId,
                comment.Content,
                comment.DatetimeCommented,
                comment.ParentCommentId,
                comment.ReplyCount), cancellationToken);
        return comments;
    }

    public async Task<PaginationItem<RatingViewModel>> GetRatingWithPaginationByUserIdAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken)
    {
        var ratings = await _bookReviewDbContext.Ratings
            .AsNoTracking()
            .Where(c => c.ReviewerId == userId)
            .OrderByDescending(x=>x.LastUpdated)
            .CreatePaginationAsync(page, rating => new RatingViewModel(
                rating.Id,
                rating.BookReviewId,
                rating.ReviewerId,
                rating.Star.Star,
                rating.DateTimeSubmitted,
                rating.LastUpdated,
                null),cancellationToken);
        return ratings;
    }

    public async Task<RatingViewModel?> GetRatingByUSerIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var bookReviews = await GetBookReviewByBookId(bookId, cancellationToken);
        if (bookReviews == null)
        {
            return null;
        }

        var rating = await _bookReviewDbContext.Ratings
            .AsNoTracking()
            .Where(x => x.BookReviewId == bookReviews.Id && x.ReviewerId == userId)
            .OrderByDescending(x=>x.LastUpdated)
            .FirstOrDefaultAsync(cancellationToken);
        return rating?.MapToViewModel();
    }

    public async Task<IReadOnlyCollection<RatingViewModel>> 
        GetRatingByBookIdsForUserAsync(Guid userId, Guid[] bookIds, CancellationToken cancellationToken = default)
    {
        var query = _bookReviewDbContext
            .BookReviews.AsNoTracking()
            .Where(x => bookIds.Contains(x.BookId))
            .Join(_bookReviewDbContext.Ratings,
                o => o.Id,
                b => b.BookReviewId,
                (o,b)=>new RatingViewModel(
                    b.Id,
                    b.BookReviewId,
                    b.ReviewerId,
                    b.Star.Star,
                    b.DateTimeSubmitted,
                    b.LastUpdated,
                    o.BookId
                    ));
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CommentViewModel>> GetAllCommentByUserIdAndBookIdAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var bookReviews = await GetBookReviewByBookId(bookId, cancellationToken);
        if (bookReviews == null)
        {
            return [];
        }

        var comment = await _bookReviewDbContext.Comments
            .AsNoTracking()
            .Where(x => x.BookReviewId == bookReviews.Id && x.ReviewerId == userId)
            .OrderByDescending(x=>x.DatetimeCommented)
            .ToListAsync(cancellationToken);
        return comment.MapToViewModel();
    }

    public async Task<IReadOnlyCollection<BookReviewViewModel>> GetBookReviewHasTopViewBookAsync(int top, CancellationToken cancellationToken= default)
    {
        var query =
            _bookReviewDbContext.BookReviews.AsNoTracking()
                .OrderByDescending(x => x.ViewCount)
                .Take(top);
        var result = await query.ToListAsync(cancellationToken);
        return result.Select(x => x.MapToViewModel()).ToList();
    }

    public async Task<PaginationItem<CommentViewModel>> GetCommentReplyWithPaginationAsync(Guid commentReplyId, PaginationRequest page,
        CancellationToken cancellationToken = default)
    {
        var comment = await _bookReviewDbContext.Comments
            .AsNoTracking()
            .Where(x => x.ParentCommentId == commentReplyId)
            .OrderByDescending(x => x.DatetimeCommented)
            .CreatePaginationAsync(page, comment => new CommentViewModel(
                comment.Id,
                comment.BookReviewId,
                comment.ReviewerId,
                comment.Content,
                comment.DatetimeCommented,
                comment.ParentCommentId,
                comment.ReplyCount
                ), cancellationToken);
        return comment;
    }
}
