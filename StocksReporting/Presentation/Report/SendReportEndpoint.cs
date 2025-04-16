using ErrorOr;
using StocksReporting.Application.Report;
using Microsoft.AspNetCore.Mvc;

namespace StocksReporting.Presentation.Report;

public class SendReportEndpoint
{
    [HttpPost("reports/send")]
    public static async Task<IResult> SendReportAsync(
        [FromBody] Request request,
        [FromServices] ReportEmailService reportEmailService)
    {
        var result = await reportEmailService.SendReportAsync(request.FilePath, request.EmailIds);

        return result.Match(
            value => Results.Ok(
                new Response(
                    new Response.ReportSent(
                        value.FilePath,
                        value.SentTo
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }

    public record Request(string FilePath, List<Guid> EmailIds);

    public record Response(Response.ReportSent Report)
    {
        public record ReportSent(string FilePath, List<string> SentTo);
    }
}
