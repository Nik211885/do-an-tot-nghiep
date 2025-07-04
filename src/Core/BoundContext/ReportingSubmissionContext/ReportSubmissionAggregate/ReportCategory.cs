using Core.ValueObjects;

namespace Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;

public class ReportCategory : ValueObject
{
    public string CategoryName { get; private set; }
    public string Description { get; private set; }
    protected ReportCategory(){}
    private ReportCategory(string categoryName, string description)
    {
        CategoryName = categoryName;
        Description = description;
    }

    public static ReportCategory Create(string categoryName, string description)
    {
        return new ReportCategory(categoryName, description);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CategoryName;
        yield return Description;
    }
}
