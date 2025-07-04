using Core.Entities;

namespace Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;

public class Report : BaseEntity
{
    public ReportContent Content { get; private set; }
    public ReportCategory Category { get; private set; }
    public DateTimeOffset CreateDateTime { get; private set; }
    public DateTimeOffset UpdateDateTime { get; private set; }
    protected Report(){}

    private Report(string categoryName, string categoryDescription, string title, string body, string attachment)
    {
        var report = ReportContent.Create(title, body, attachment);
        var reportCategory = ReportCategory.Create(categoryName, categoryDescription);
        Content = report;
        Category = reportCategory;
        CreateDateTime = DateTimeOffset.UtcNow;
        UpdateDateTime = DateTimeOffset.UtcNow;
    }

    public static Report Create(string categoryName, string categoryDescription, string title, string body,
        string attachment)
    {
        return new Report(categoryName, categoryDescription, title, body, attachment);
    }

    public void UpdateCategory(string categoryName, string categoryDescription)
    {
        var category = ReportCategory.Create(categoryName, categoryDescription);
        Category = category;
        UpdateDateTime = DateTimeOffset.UtcNow;
    }

    public void UpdateContent(string title, string body, string attachment)
    {
        var report = ReportContent.Create(title, body, attachment);
        Content = report;
        UpdateDateTime = DateTimeOffset.UtcNow;
    }
}
