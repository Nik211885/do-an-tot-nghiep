using Application.Interfaces.Notification;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Services.Notification;

public class EmailSender(IOptions<MailSettingConfiguration> mailSettingConfiguration,
    ILogger<EmailSender> logger) 
    : IEmailSender
{
    private readonly ILogger<EmailSender> _logger = logger;
    private readonly MailSettingConfiguration _mailSettingConfiguration = mailSettingConfiguration.Value;
    public async Task SendEmailAsync(string to, string body, string subject, string? nameTo = null, bool isLink = false)
    {
        try
        {
            var emailMessage = new MimeMessage();
            var emailForm = new MailboxAddress(_mailSettingConfiguration.Name, _mailSettingConfiguration.EmailId);
            emailMessage.From.Add(emailForm);
            nameTo ??= to.Split("@")[0];
            var emailTo = new MailboxAddress(nameTo, to);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            if (!isLink)
            {
                bodyBuilder.TextBody = body;
            }
            else
            {
                bodyBuilder.HtmlBody = body;
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(_mailSettingConfiguration.Host, _mailSettingConfiguration.Port, _mailSettingConfiguration.UseSSL);
            await mailClient.AuthenticateAsync(_mailSettingConfiguration.EmailId, _mailSettingConfiguration.PasswordApplication);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);
            mailClient.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(@"Has problem with send email Exception 
                            with content send {to} and body is {body}
                            subject {subject}, name is {nameTo}, isLink {isLink} {ex}",
                            ex, to, body,subject,nameTo, isLink);
        }
    }
}
