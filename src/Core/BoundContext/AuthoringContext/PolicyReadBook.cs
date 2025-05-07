using System.Text.Json.Serialization;
using Core.Exception;
using Core.Message;
using Core.ValueObjects;

namespace Core.BoundContext.BookManagement.BookAggregate;

public class PolicyReadBook : ValueObject
{
    /// <summary>
    ///  Make simple price is VND 
    /// </summary>
    public decimal? Price { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public BookPolicy Policy { get; private set; }

    private PolicyReadBook(BookPolicy policy, decimal? price)
    {
        Policy = policy;
        Price = price;
    }

    public static PolicyReadBook CreatePolicy(BookPolicy policy, decimal? price)
    {
        if (policy == BookPolicy.Paid && price is null or < 0)
        {
            throw new BadRequestException(BookManagementContextMessage.BookPaidPolicyButDontAddPrice);
        }

        if (policy != BookPolicy.Paid && price is not null)
        {
            throw new BadRequestException(BookManagementContextMessage.BookNotPaidPolicyButAddPrice);
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
/// <summary>
/// 
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookPolicy
{
    /// <summary>
    /// 
    /// </summary>
    Free = 1,
    /// <summary>
    /// 
    /// </summary>
    Paid = 2,
    /// <summary>
    /// 
    /// </summary>
    Subscription = 3,
}
