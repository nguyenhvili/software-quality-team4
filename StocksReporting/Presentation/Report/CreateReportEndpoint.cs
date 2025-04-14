using ErrorOr;
using StocksReporting.Application.Report;
using Wolverine;
using Wolverine.Http;

namespace StocksReporting.Presentation.Report;

public class CreateReportEndpoint
{
    [WolverinePost("reports")]
    public static async Task<IResult> CreateReportAsync(Request request, IMessageBus sender)
    {
        var command = new CreateReportCommand(request.FilePath, request.CreatedAt);

        var result = await sender.InvokeAsync<ErrorOr<CreateReportCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new Response(
                    new Response.CreatedReport(
                        value.Report.Id,
                        value.Report.FilePath,
                        value.Report.CreatedAt
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }

    public record Request(string FilePath, DateTime? CreatedAt);

    public record Response(Response.CreatedReport Report)
    {
        public record CreatedReport(Guid Id, string FilePath, DateTime CreatedAt);
    }
}
