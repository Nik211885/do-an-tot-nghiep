using Core.ValueObjects;

namespace Core.BoundContext.BookAuthoringContext.BookAggregate;

public class Tag : ValueObject
{
    /// <summary>
    /// 
    /// </summary>
    public string TagName { get; private set; }
    protected Tag(){}
    private Tag(string tagName)
    {
        TagName = tagName;
    }

    public static Tag CreateTag(string tagName)
    {
        // add more condition when author add tag like rule 
        return new Tag(tagName);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TagName; 
    }
}
