/*
using Core.ValueObjects;

namespace Core.BoundContext.ModerationContext.BookApprovalAggregate;

public class DigitalSignature : ValueObject
{
    public string SignatureValue { get; private set; }
    public string SignatureAlgorithm { get; private set; }
    public DateTimeOffset SigningDateTime { get; private set; }
    protected DigitalSignature(){}
    private DigitalSignature(string signatureValue, string signatureAlgorithm)
    {
        SignatureValue = signatureValue;
        SignatureAlgorithm = signatureAlgorithm;
        SigningDateTime = DateTimeOffset.UtcNow;
    }

    public static DigitalSignature Create(string signatureValue, string signatureAlgorithm)
    {
        return new DigitalSignature(signatureValue, signatureAlgorithm);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SignatureValue;
        yield return SignatureAlgorithm;
        yield return SigningDateTime;
    }
}
*/
