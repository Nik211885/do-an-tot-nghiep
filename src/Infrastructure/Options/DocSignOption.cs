namespace Infrastructure.Options;

[KeyOptions("DocSign")]
public class DocSignOption
{
    public string IntegrationKey { get; set; }
    public string SecretKey { get; set; }
    public string UserId { get; set; }
    public string AccountId { get; set; }
    public string BaseUrl { get; set; }
    public string AuthServerUrl { get; set; }
    public string RedirectUri { get; set; }
    public string PrivateKeyPath { get; set; }
    public string IntermediaryEmail { get; set; }
    public string IntermediaryName { get; set; }
}
