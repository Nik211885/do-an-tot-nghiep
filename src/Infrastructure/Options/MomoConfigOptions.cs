namespace Infrastructure.Options;

[KeyOptions("MoMoConfig")]
public class MomoConfigOptions
{
    public string PartnerCode { get; set; }
    public string ReturnUrl { get; set; }
    public string IpnUrl { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string PaymentUrl { get; set; }
}
