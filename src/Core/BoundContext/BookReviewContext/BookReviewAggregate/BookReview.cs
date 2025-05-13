using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class BookReview : BaseEntity, IAggregateRoot
{
    private Guid _bookId;
    public int View { get; private set; }
    private List<Rating>? _ratings;
    private List<Comment>? _comments;
    public IReadOnlyCollection<Rating> Ratings => _ratings ?? [];
    public IReadOnlyCollection<Comment> Comments => _comments ?? [];
    private BookReview(Guid bookId, Rating? rating, Comment? comment)
    {
        _bookId = bookId;
        if (rating is not null)
        {
            _ratings ??= [];
            _ratings.Add(rating);
        }

        if (comment is null)
        {
            return;
        }

        View = 0;
        _comments ??= [];
        _comments.Add(comment);
    }

    public static BookReview CreateBookReview(Guid bookId, Rating? rating, Comment? comment)
    {
        return new BookReview(bookId, rating, comment);
    }

    public void AddRating(Guid reviewerId, int star)
    {
        var ratingExits = _ratings?.FirstOrDefault(r=>r.ReviewerId == reviewerId);
        if (ratingExits is not null)
        {
            throw new BadRequestException(BookReviewContextMessage.JustHaveOneRatingInTheBook);
        }
        var rating = Rating.Create(reviewerId, new RatingStar(star));
        _ratings ??= [];
        _ratings.Add(rating);
    }

    public void AddComment(Guid reviewerId, string commentContent)
    {
        var comment = Comment.Create(Id, reviewerId, commentContent, null);
        _comments ??= [];
        _comments.Add(comment);
        RaiseDomainEvent(new CommentedBookDomainEvent(comment.Id, _bookId, commentContent));
    }

    public void RemoveComment(Guid commentId)
    {
        var comment = _comments?.FirstOrDefault(c=>c.Id == commentId);
        if (comment is null)
        {
            throw new BadRequestException(BookReviewContextMessage.CanNotFindCommentId);
        }
        _comments?.Remove(comment);
    }

    public void UpdateRating(Guid reviewerId, int star)
    {
        var rating = _ratings?.FirstOrDefault(r => r.ReviewerId == Id);
        if (rating == null)
        {
            throw new BadRequestException(BookReviewContextMessage.CanNotFindYourRating);
        }
        var newRating = Rating.Create(reviewerId, new RatingStar(star));
        _ratings?.Remove(rating);
        _ratings?.Add(newRating);
    }

    public void AddView()
    {
        View += 1;
    }
    public double CalculateAverageRating()
    {
        if (_ratings == null || !_ratings.Any())
            return 0;
        return _ratings.Average(r => r.Star.Star);
    }
    public int TotalRatings => _ratings?.Count ?? 0;
}
