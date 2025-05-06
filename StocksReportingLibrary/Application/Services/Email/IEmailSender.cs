namespace StocksReportingLibrary.Application.Services.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body, string[] attachments);
}
