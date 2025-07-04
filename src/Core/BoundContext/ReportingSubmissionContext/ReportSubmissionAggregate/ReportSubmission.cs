using Core.Entities;
using Core.Events.ReportSubmission;
using Core.Interfaces;

namespace Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;

public class ReportSubmission 
    : BaseEntity, IAggregateRoot
{
    public Report Report { get; private set; }
    public Submitter Submitter { get; private set; }
    public SubmissionStatus Status { get; private set; }
    public DateTimeOffset SubmissionTime { get; private set; }
    protected ReportSubmission(){}
    private ReportSubmission(string categoryName, string categoryDescription, string title, string body,
        string attachment,
        Guid? submitterId, string name, string email)
    {
        var report = Report.Create(categoryName, categoryDescription, title, body, attachment);
        var submitter = ReportSubmissionAggregate.Submitter.Create(submitterId, name, email);
        Submitter = submitter;
        Report = report;
        Status = SubmissionStatus.Draft;
        SubmissionTime = DateTimeOffset.UtcNow;
    }

    public static ReportSubmission Create(string categoryName, string categoryDescription, string title, string body,
        string attachment,
        Guid? submitterId, string name, string email)
    {
        var report =  new ReportSubmission(categoryName, categoryDescription, title, body, attachment, submitterId, name, email);
        return report;
    }

    public void Submit()
    {
        Status = SubmissionStatus.Sent;
        RaiseDomainEvent(new UserSubmittedReportDomainEvent(Id));
    }

    public void Cancel()
    {
        Status = SubmissionStatus.Failed;
        RaiseDomainEvent(new CancelledSubmittedReportDomainEvent(Id));
    }
}
