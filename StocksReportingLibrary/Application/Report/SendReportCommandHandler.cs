using StocksReportingLibrary.Presentation.Report;
using ErrorOr;
using StocksReportingLibrary.Application.Services.Email;

namespace StocksReportingLibrary.Application.Report;

public class SendReportHandler
{
    private readonly IReportEmailService _reportEmailService;

    public SendReportHandler(IReportEmailService reportEmailService)
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
