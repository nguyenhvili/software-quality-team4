using ErrorOr;
using Microsoft.AspNetCore.Http;
using StocksReportingLibrary.Application.Common;
using StocksReportingLibrary.Application.Report;
using Wolverine;
using Wolverine.Http;

namespace StocksReportingLibrary.Presentation.Report;

public class ListReportsEndpoint
{
    [WolverineGet("reports")]
    public static async Task<IResult> ListReportsAsync(int page, int pageSize, IMessageBus sender)
    {
        var pagingResult = Paging.Create(page, pageSize);
        if (pagingResult.IsError)
        {
            return Results.BadRequest(pagingResult.Errors.Select(e => e.Code));
        }
        
        var command = new ListReportsCommand(pagingResult.Value);

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
