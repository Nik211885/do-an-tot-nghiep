namespace Core.Events.ReportSubmission;

public class UserSubmittedReportDomainEvent(Guid submissionId) : IEvent
{
    public Guid SubmissionId { get;} =submissionId;
}
