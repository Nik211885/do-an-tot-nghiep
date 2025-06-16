namespace Infrastructure.Options;
[KeyOptions("MailSettings")]
public class MailSettingOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public string EmailId { get; set; }
    public string UserName { get; set; }
    public string PasswordApplication { get; set; }
    public bool UseSSL { get; set; }
}
