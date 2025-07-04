using Core.BoundContext.ModerationContext.BookApprovalAggregate;

namespace Domain.UnitTest.BoundContext.ModerationContext;

public class DigitalSignatureTests
{
    [Fact]
    public void Create_Should_Succeed_With_Valid_Data()
    {
        var signature = DigitalSignature.Create("sig", "alg");
        Assert.Equal("sig", signature.SignatureValue);
        Assert.Equal("alg", signature.SignatureAlgorithm);
        Assert.True(signature.SigningDateTime <= DateTimeOffset.UtcNow);
    }
}
