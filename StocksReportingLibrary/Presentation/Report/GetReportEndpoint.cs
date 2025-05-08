using StocksReportingLibrary.Application.Report;
using Wolverine;
using Wolverine.Http;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace StocksReportingLibrary.Presentation.Report;

public class GetReportEndpoint
{
    private readonly ILogger<GetReportEndpoint> _logger;

    public GetReportEndpoint(ILogger<GetReportEndpoint> logger)
    {
        _logger = logger;
    }

    [WolverineGet("reports/{id}")]
    public async static Task<IResult> GetReportAsync(Guid id, IMessageBus sender, ILogger<GetReportEndpoint> logger)
    {
        logger.LogInformation("Received request to get report with Id: {Id}", id);

        var command = new GetReportCommand(id);

        var result = await sender.InvokeAsync<ErrorOr<GetReportCommand.Result>>(command);

        return result.Match(
            success =>
            {
                logger.LogInformation("Report retrieved successfully. Id: {Id}", id);
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
            errors =>
            {
                logger.LogWarning("Report with Id: {Id} not found. Errors: {Errors}", id, errors);
                return Results.NotFound(errors.Select(e => e.Code));
            }
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
