using Core.Entities;
using Core.Events.BookReviewContext;
using Core.Exception;
using Core.Interfaces;
using Core.Message;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class BookReview : BaseEntity, IAggregateRoot
{
    public Guid _bookId;
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
        var rating = Rating.Create(Id, reviewerId, new RatingStar(star));
        _ratings ??= [];
        _ratings.Add(rating);
    }

    public void AddComment(Guid reviewerId, string commentContent)
    {
        var comment = Comment.Create(Id, reviewerId, commentContent);
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
}
