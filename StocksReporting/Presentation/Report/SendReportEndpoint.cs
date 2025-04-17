using ErrorOr;
using StocksReporting.Application.Report;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;

namespace StocksReporting.Presentation.Report;

public class SendReportEndpoint
{
    [WolverinePost("/reports/send")]
    public static async Task<IResult> SendReportAsync(
        [FromBody] Request request,
        [FromServices] IMessageBus bus)
    {
        var result = await bus.InvokeAsync<ErrorOr<Response.SentReport>>(
            new SendReportCommand(request.FilePath, request.EmailIds)
        );

        return result.Match(
            value => Results.Ok(new Response(value)),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }

    public record Request(string FilePath, List<Guid> EmailIds);

    public record Response(Response.SentReport Report)
    {
        public record SentReport(string FilePath, List<string> SentTo);
    }
}
