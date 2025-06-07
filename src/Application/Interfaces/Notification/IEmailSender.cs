namespace Application.Interfaces.Notification;

public interface IEmailSender 
{
    Task SendEmailAsync(string to, string body, string subject,string? messageLink, string? nameTo = null);
}
