using ErrorOr;
using StocksReportingLibrary.Application.Report;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace StocksReportingLibrary.Presentation.Report;

public class SendReportEndpoint
{
    private readonly ILogger<SendReportEndpoint> _logger;

    public SendReportEndpoint(ILogger<SendReportEndpoint> logger)
    {
        _logger = logger;
    }

    [WolverinePost("/reports/send")]
    public async static Task<IResult> SendReportAsync(
        [FromBody] Request request,
        [FromServices] IMessageBus bus,
        ILogger<SendReportEndpoint> logger)
    {
        logger.LogInformation("Received request to send report. ReportId: {ReportId}, EmailCount: {EmailCount}",
            request.ReportId, request.EmailIds.Count);

        var result = await bus.InvokeAsync<ErrorOr<Response.SentReport>>(
            new SendReportCommand(request.ReportId, request.EmailIds)
        );

        return result.Match(
            value =>
            {
                logger.LogInformation("Report sent successfully. ReportId: {ReportId}, SentTo: {SentTo}",
                    value.ReportId, string.Join(",", value.SentTo));
                return Results.Ok(new Response(value));
            },
            errors =>
            {
                logger.LogWarning("Failed to send report. Errors: {Errors}", errors);
                return Results.BadRequest(errors.Select(e => e.Code));
            }
        );
    }

    public record Request(Guid ReportId, List<Guid> EmailIds);

    public record Response(Response.SentReport Report)
    {
        public record SentReport(Guid ReportId, List<string> SentTo);
    }
}
