using System.Text.Json.Serialization;
using Core.Exception;
using Core.Message;
using Core.ValueObjects;

namespace Core.BoundContext.BookAuthoringContext.BookAggregate;
/// <summary>
///     Just apply policy free, paid, subscription
///     if free read all book
///     if subscription user need follow author
///     paid money new has read book
///     don't support policy in chapter 
/// </summary>
public class PolicyReadBook : ValueObject
{
    public decimal? Price { get; private set; }
    public BookPolicy Policy { get; private set; }
    
    private PolicyReadBook(BookPolicy policy, decimal? price)
    {
        Policy = policy;
        Price = price;
    }
    protected PolicyReadBook(){}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="policy"></param>
    /// <param name="price"></param>
    /// <returns></returns>
    public static PolicyReadBook CreatePolicy(BookPolicy policy, decimal? price)
    {
        if (policy == BookPolicy.Paid && price is null or < 0)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.BookPaidPolicyButDontAddPrice);
        }

        if (policy != BookPolicy.Paid && price > 0)
        {
            ThrowHelper.ThrowIfBadRequest(BookAuthoringContextMessage.BookNotPaidPolicyButAddPrice);
        }
        return new PolicyReadBook(policy, price);
    }

    public PolicyReadBook UpdatePolicy(BookPolicy policy, decimal? price)
    {
        return CreatePolicy(policy, price);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Policy;
        if (Price is not null)
        {
            yield return Price;
        }
    }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookPolicy
{
    Free = 1,
    Paid = 2,
    Subscription = 3,
}
