using Core.ValueObjects;

namespace Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;

public class ReportContent : ValueObject
{
    public string Title { get; private set; }
    public string Body { get; private set; }
    public string Attachments  {get; private set;}

    private ReportContent(string title, string body, string attachments)
    {
        Title = title;
        Body = body;
        Attachments = attachments;
    }

    public static ReportContent Create(string title, string body, string attachments)
    {
        return new ReportContent(title, body, attachments);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
