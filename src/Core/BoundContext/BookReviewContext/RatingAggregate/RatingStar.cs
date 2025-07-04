using Core.Entities;
using Core.Exception;
using Core.Message;
using Core.ValueObjects;

namespace Core.BoundContext.BookReviewContext.RatingAggregate;

public class RatingStar : ValueObject
{
    public int Star { get; private set; }
    public static RatingStar Create(int star)
    {
        if (star is <= 0 or > 5)
        {
            ThrowHelper.ThrowIfBadRequest(BookReviewContextMessage.RatingStarNotFormat);
        }
        // add rule for comment
        return new RatingStar(star);
    }
    protected RatingStar(){}
    private RatingStar(int star)
    {
        Star = star;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Star;
    }
}
