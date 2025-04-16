using ErrorOr;
using StocksReporting.Application.Common.Interfaces;
using StocksReporting.Application.Services;
using StocksReporting.Domain.Common;
using StocksReporting.Domain.Email;

namespace StocksReporting.Application.Report;

public class ReportEmailService
{
    private readonly IRepository<Domain.Email.Email> _emailRepository;
    private readonly IEmailSender _emailSender;

    public ReportEmailService(
        IRepository<Domain.Email.Email> emailRepository,
        IEmailSender emailSender)
    {
        _emailRepository = emailRepository;
        _emailSender = emailSender;
    }

    public async Task<ErrorOr<SendReportResult>> SendReportAsync(string filePath, List<Guid> emailIds)
    {
        var emails = new List<string>();

        foreach (var emailId in emailIds)
        {
            var emailEntity = await _emailRepository.GetByIdAsync(emailId);
            if (emailEntity == null) continue;
            emails.Add(emailEntity.EmailValue.Value);
        }

        if (emails.Count == 0)
        {
            return Error.Validation("Email.NoneFound", "No valid emails found");
        }

        foreach (var email in emails)
        {
            await _emailSender.SendEmailAsync(
                to: email,
                subject: "Weekly Report Export",
                body: $"Please find the attached report.",
                attachments: new[] { filePath }
            );
        }

        return new SendReportResult(filePath, emails);
    }

    public record SendReportResult(string FilePath, List<string> SentTo);
}
