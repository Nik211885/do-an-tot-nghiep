using Core.Entities;

namespace Core.BoundContext.ReportingSubmissionContext.ReportSubmissionAggregate;

public class Submitter : BaseEntity
{
    public Guid SubmitterId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    protected Submitter(){}
    private Submitter(Guid? submitterId, string name, string email)
    {
        SubmitterId = submitterId ?? Guid.NewGuid();
        Name = name;
        Email = email;
    }

    public static Submitter Create(Guid? submitterId, string name, string email)
    {
        return new Submitter(submitterId, name, email);
    }

    public void UpdateInfo(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
