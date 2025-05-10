using ErrorOr;
using StocksReportingLibrary.Application.Services.Email;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Report.ValueObjects;
using static StocksReportingLibrary.Application.Services.Email.IReportEmailService;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.Email;

public class ReportEmailService : IReportEmailService
{
    private readonly IQueryObject<Domain.Email.Email> _emailQuery;
    private readonly IQueryObject<Domain.Report.Report> _reportQuery;
    private readonly IEmailQueue _emailQueue;
    private readonly ILogger<ReportEmailService> _logger;

    public ReportEmailService(
        IQueryObject<Domain.Email.Email> emailQuery,
        IQueryObject<Domain.Report.Report> reportQuery,
        IEmailQueue emailQueue,
        ILogger<ReportEmailService> logger)
    {
        _emailQuery = emailQuery;
        _reportQuery = reportQuery;
        _emailQueue = emailQueue;
        _logger = logger;
    }

    public async Task<ErrorOr<SendReportResult>> SendReportAsync(Guid reportId, List<Guid> emailIds)
    {
        _logger.LogInformation("Sending report with ID: {ReportId} to {EmailCount} emails.",
            reportId, emailIds.Count);

        var ids = emailIds.ToHashSet();

        var report =
            (await _reportQuery
                .Filter(r => r.Id == ReportId.Create(reportId))
                .ExecuteAsync())
            .SingleOrDefault();

        if (report is null)
        {
            _logger.LogWarning("Report with ID {ReportId} not found.", reportId);
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
            _logger.LogWarning("No valid emails found for sending the report.");
            return Error.Validation("Email.NoneFound", "No valid emails found");
        }

        try
        {
            foreach (var email in emails)
            {
                await _emailQueue.EnqueueEmailAsync(
                   email,
                    "Weekly Report Export",
                   "Please find the attached report.",
                   [report.ReportPathValue.PathValue]
                );
                _logger.LogDebug("Enqueued sending report email to: {Email}", email);
            }

            _logger.LogInformation("Report with ID: {ReportId} enqueued to {SentEmailCount} emails successfully.",
                reportId, emails.Count);
            return new SendReportResult(reportId, emails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending report with ID: {ReportId}", reportId);
            return Error.Failure("An error occurred while sending the report.");
        }
    }
}
