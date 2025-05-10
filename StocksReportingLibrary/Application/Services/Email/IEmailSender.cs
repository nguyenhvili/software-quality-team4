using ErrorOr;

namespace StocksReportingLibrary.Application.Services.Email;

public interface IEmailSender
{
    Task<ErrorOr<Success>> SendEmailAsync(string to, string subject, string body, string[] attachments);
}
