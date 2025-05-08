using StocksReportingLibrary.Presentation.Report;
using ErrorOr;
using StocksReportingLibrary.Application.Services.Email;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Report;

public class SendReportHandler
{
    private readonly IReportEmailService _reportEmailService;
    private readonly ILogger<SendReportHandler> _logger;

    public SendReportHandler(IReportEmailService reportEmailService, ILogger<SendReportHandler> logger)
    {
        _reportEmailService = reportEmailService;
        _logger = logger;
    }

    public async Task<ErrorOr<SendReportEndpoint.Response.SentReport>> Handle(SendReportCommand command)
    {
        _logger.LogInformation("SendReportCommandHandler for ReportId: {ReportId}, EmailIds: {EmailIds}",
            command.ReportId, string.Join(",", command.EmailIds));

        var result = await _reportEmailService.SendReportAsync(command.ReportId, command.EmailIds);

        if (result.IsError)
        {
            _logger.LogError("Failed to send report: {Errors}", result.Errors);
            return result.Errors;
        }

        var value = result.Value;

        _logger.LogInformation("Report sent successfully. ReportId: {ReportId}, SentTo: {SentTo}",
            value.ReportId, string.Join(",", value.SentTo));

        return new SendReportEndpoint.Response.SentReport(
            value.ReportId,
            value.SentTo
        );
    }
}
