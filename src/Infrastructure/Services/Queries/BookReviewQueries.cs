using Application.BoundContext.BookReviewContext.Queries;
using Application.BoundContext.BookReviewContext.ViewModel;
using Application.Models;
using Infrastructure.Data.DbContext;
using Infrastructure.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Queries;

public class BookReviewQueries(BookReviewDbContext bookReviewDbContext) 
    : IBookReviewQueries
{
    private readonly BookReviewDbContext _bookReviewDbContext = bookReviewDbContext;
    public async Task<BookReviewViewModel?> GetBookReviewByBookId(Guid bookId, CancellationToken cancellationToken = default)
    {
        var result = await _bookReviewDbContext.BookReviews
            .AsNoTracking()
            .Where(bookReview => bookReview.BookId == bookId)
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
            .Where(x=>x.BookReviewId == bookReviews.Id)
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
            .CreatePaginationAsync(page, rating => new RatingViewModel(
                rating.Id,
                rating.BookReviewId,
                rating.ReviewerId,
                rating.Star.Star,
                rating.DateTimeSubmitted,
                rating.LastUpdated
            ), cancellationToken);
        return ratingWithPagination;
    }

    public async Task<PaginationItem<CommentViewModel>> GetCommentWithPaginationByUserIdAsync(Guid userId, PaginationRequest page, CancellationToken cancellationToken)
    {
        var comments = await _bookReviewDbContext.Comments
            .AsNoTracking()
            .Where(x=>x.ReviewerId == userId)
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
            .CreatePaginationAsync(page, rating => new RatingViewModel(
                rating.Id,
                rating.BookReviewId,
                rating.ReviewerId,
                rating.Star.Star,
                rating.DateTimeSubmitted,
                rating.LastUpdated), cancellationToken);
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
            .FirstOrDefaultAsync(cancellationToken);
        return rating?.MapToViewModel();
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
            .ToListAsync(cancellationToken);
        return comment.MapToViewModel();
    }
}
