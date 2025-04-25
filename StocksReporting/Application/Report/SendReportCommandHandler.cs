using StocksReporting.Presentation.Report;
using ErrorOr;

namespace StocksReporting.Application.Report;

public class SendReportHandler
{
    private readonly ReportEmailService _reportEmailService;

    public SendReportHandler(ReportEmailService reportEmailService)
    {
        _reportEmailService = reportEmailService;
    }

    public async Task<ErrorOr<SendReportEndpoint.Response.SentReport>> Handle(SendReportCommand command)
    {
        var result = await _reportEmailService.SendReportAsync(command.ReportId, command.EmailIds);

        if (result.IsError)
            return result.Errors;

        var value = result.Value;

        return new SendReportEndpoint.Response.SentReport(
            value.ReportId,
            value.SentTo
        );
    }
}
