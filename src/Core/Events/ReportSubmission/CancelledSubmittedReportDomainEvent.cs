namespace Core.Events.ReportSubmission;

public class CancelledSubmittedReportDomainEvent(Guid submissionId) : IEvent
{
    public Guid SubmittedReportId { get; } = submissionId;
}
