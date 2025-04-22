using ErrorOr;
using StocksReporting.Application.Report;
using Wolverine;
using Wolverine.Http;

namespace StocksReporting.Presentation.Report;

public class ListReportsEndpoint
{
    [WolverineGet("reports")]
    public static async Task<IResult> ListReportsAsync(int page, int pageSize, IMessageBus sender)
    {
        var command = new ListReportsCommand(page, pageSize);

        var result = await sender.InvokeAsync<ErrorOr<ListReportsCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new Response(
                    value.Reports.Select(report => new Response.Report(report.Id, report.CreatedAt))
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }

    public record Response(IEnumerable<Response.Report> Reports)
    {
        public record Report(Guid Id, DateTime CreatedAt);
    }
}
