namespace StocksReporting.Application.Common.Interfaces;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, string[] attachments);
}
