namespace StocksReporting.Application.Report;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, string[] attachments);
}
