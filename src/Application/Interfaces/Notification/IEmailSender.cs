namespace Application.Interfaces.Notification;

public interface IEmailSender 
{
    Task SendEmailAsync(string to, string body, string subject, string? nameTo = null, bool isLink = false);
}
