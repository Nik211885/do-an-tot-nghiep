namespace Core.ValueObjects;

/// <summary>
///  Value object you can use record type make value object
/// </summary>
public abstract class ValueObject
{
    // Compare object when it is null
    private static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }
        return ReferenceEquals(left, null) || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !(EqualOperator(left, right));
    }
    /// <summary>
    ///     List property make compare with different object is value object
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<object> GetEqualityComponents();
    /// <summary>
    ///  Compare all value in property type in components
    ///  When two value object consider equals when it has all property in equality components have same value
    ///  This design has same data type record
    ///  When record compare all property but in value object can compare property for business
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>
    ///     Return true if two value object have same value otherwise return false
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }
    /// <summary>
    ///  It makes compare value object faster when it hashes all property in equality components 
    /// </summary>
    /// <returns>
    ///     Return code hash 
    /// </returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x.GetHashCode())
            .Aggregate((x, y) => x ^ y);
    }
    /// <summary>
    ///     Get all object has copy
    /// </summary>
    /// <returns></returns>
    public ValueObject? GetCopy()
        => this.MemberwiseClone() as ValueObject;
}
