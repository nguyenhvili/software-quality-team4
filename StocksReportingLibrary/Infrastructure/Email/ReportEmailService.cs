using ErrorOr;
using StocksReportingLibrary.Application.Services.Email;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Report.ValueObjects;
using static StocksReportingLibrary.Application.Services.Email.IReportEmailService;

namespace StocksReportingLibrary.Infrastructure.Email;

public class ReportEmailService : IReportEmailService
{
    private readonly IQueryObject<Domain.Email.Email> _emailQuery;
    private readonly IQueryObject<Domain.Report.Report> _reportQuery;
    private readonly IEmailSender _emailSender;

    public ReportEmailService(
        IQueryObject<Domain.Email.Email> emailQuery,
        IQueryObject<Domain.Report.Report> reportQuery,
        IEmailSender emailSender)
    {
        _emailQuery = emailQuery;
        _reportQuery = reportQuery;
        _emailSender = emailSender;
    }

    public async Task<ErrorOr<SendReportResult>> SendReportAsync(Guid reportId, List<Guid> emailIds)
    {
        var ids = emailIds.ToHashSet();

        var report =
            (await _reportQuery
                .Filter(r => r.Id == ReportId.Create(reportId))
                .ExecuteAsync())
            .SingleOrDefault();

        if (report is null)
        {
            return Error.Validation("Report.NotFound", "Report not found");
        }

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
                attachments: new[] { report.ReportPathValue.PathValue }
            );
        }

        return new SendReportResult(reportId, emails);
    }
}
