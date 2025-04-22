using ErrorOr;
using StocksReporting.Application.Report;
using StocksReporting.Application.Services;
using StocksReporting.Domain.Common;
using StocksReporting.Domain.Email;

namespace StocksReporting.Application.Report;

public class ReportEmailService
{
    private readonly IQueryObject<Domain.Email.Email> _emailQuery;
    private readonly IEmailSender _emailSender;

    public ReportEmailService(
        IQueryObject<Domain.Email.Email> emailQuery,
        IEmailSender emailSender)
    {
        _emailQuery = emailQuery;
        _emailSender = emailSender;
    }

    public async Task<ErrorOr<SendReportResult>> SendReportAsync(string filePath, List<Guid> emailIds)
    {
        var ids = emailIds.ToHashSet();

        // Force evaluation of the query in memory
        var emailEntities = (await _emailQuery.ExecuteAsync())
            .Where(e => ids.Contains(e.Id.Value))
            .ToList();

        var emails = emailEntities
            .Select(e => e.EmailValue.Value)
            .Distinct()
            .ToList();

        if (emails.Count == 0)
        {
            return Error.Validation("Email.NoneFound", "No valid emails found");
        }

        foreach (var email in emails)
        {
            await _emailSender.SendEmailAsync(
                to: email,
                subject: "Weekly Report Export",
                body: "Please find the attached report.",
                attachments: new[] { filePath }
            );
        }

        return new SendReportResult(filePath, emails);
    }

    public record SendReportResult(string FilePath, List<string> SentTo);
}
