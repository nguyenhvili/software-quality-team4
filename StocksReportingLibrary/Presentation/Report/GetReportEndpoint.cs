using StocksReportingLibrary.Application.Report;
using Wolverine;
using Wolverine.Http;
using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace StocksReportingLibrary.Presentation.Report;

public class GetReportEndpoint
{
    [WolverineGet("reports/{id}")]
    public static async Task<IResult> GetReportAsync(Guid id, IMessageBus sender)
    {
        var command = new GetReportCommand(id);

        var result = await sender.InvokeAsync<ErrorOr<GetReportCommand.Result>>(command);
        
        return result.Match(
            success =>
            {
                var r = success.Report;
                var dto = new Response(
                    r.Id,
                    r.FilePath,
                    r.CreatedAt,
                    r.Holdings.Select(h => new Response.Holding(
                        h.Id,
                        h.CompanyName,
                        h.Ticker,
                        h.Shares,
                        h.SharesPercent,
                        h.Weight
                    ))
                );
                return Results.Ok(dto);
            },
            errors => Results.NotFound(errors.Select(e => e.Code))
        );
    }
    
    public record Response(
        Guid Id,
        string FilePath,
        DateTime CreatedAt,
        IEnumerable<Response.Holding> Holdings
    )
    {
        public record Holding(
            Guid Id,
            string CompanyName,
            string Ticker,
            decimal Shares,
            decimal SharesPercent,
            decimal Weight
        );
    }
}
