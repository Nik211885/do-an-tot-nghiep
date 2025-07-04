using Application.Interfaces.Notification;
using Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Infrastructure.Services.Notification;

public class EmailSender(IOptions<MailSettingOptions> mailSettingConfiguration,
    ILogger<EmailSender> logger) 
    : IEmailSender
{
    private readonly ILogger<EmailSender> _logger = logger;
    private readonly MailSettingOptions _mailSettingOptions = mailSettingConfiguration.Value;
    public async Task SendEmailAsync(string to, string body, string subject, string? messaggeLink , string? nameTo = null)
    {
        try
        {
            var emailMessage = new MimeMessage();
            var emailForm = new MailboxAddress(_mailSettingOptions.Name, _mailSettingOptions.EmailId);
            emailMessage.From.Add(emailForm);
            nameTo ??= to.Split("@")[0];
            var emailTo = new MailboxAddress(nameTo, to);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = subject;
            var bodyBuilder = new BodyBuilder { TextBody = body };
            if (!string.IsNullOrWhiteSpace(messaggeLink))
            {
                bodyBuilder.HtmlBody = messaggeLink;
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            var mailClient = new SmtpClient();
            await mailClient.ConnectAsync(_mailSettingOptions.Host, _mailSettingOptions.Port, _mailSettingOptions.UseSSL);
            await mailClient.AuthenticateAsync(_mailSettingOptions.EmailId, _mailSettingOptions.PasswordApplication);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);
            mailClient.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(@"Has problem with send email Exception 
                            with content send {to} and body is {body}
                            subject {subject}, name is {nameTo} {ex}",
                            ex, to, body,subject,nameTo);
        }
    }
}
