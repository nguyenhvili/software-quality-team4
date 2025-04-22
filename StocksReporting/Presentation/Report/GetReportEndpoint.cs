using StocksReporting.Application.Report;
using Wolverine;
using Wolverine.Http;
using ErrorOr;

namespace StocksReporting.Presentation.Report;

public class GetReportEndpoint
{
    [WolverineGet("reports/{id}")]
    public static async Task<IResult> ListReportsAsync(Guid id, IMessageBus sender)
    {
        var command = new GetReportCommand(id);

        var result = await sender.InvokeAsync<ErrorOr<GetReportCommand.Result>>(command);

        return result.Match(
            Results.Ok,
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}