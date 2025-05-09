using Core.Entities;
using Core.Exception;
using Core.Message;
using Core.ValueObjects;

namespace Core.BoundContext.BookReviewContext.BookReviewAggregate;

public class RatingStar(int start ) : ValueObject
{
    public int Star { get; } = start;

    public RatingStar Create(int star)
    {
        if (star is <= 0 or > 5)
        {
            throw new BadRequestException(BookReviewContextMessage.RatingStarNotFormat);
        }
        // add rule for comment
        return new RatingStar(star);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Star;
    }
}
