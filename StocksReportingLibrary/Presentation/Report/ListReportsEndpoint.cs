using ErrorOr;
using Microsoft.AspNetCore.Http;
using StocksReportingLibrary.Application.Common;
using StocksReportingLibrary.Application.Report;
using Wolverine;
using Wolverine.Http;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Presentation.Report;

public class ListReportsEndpoint
{
    private readonly ILogger<ListReportsEndpoint> _logger;

    public ListReportsEndpoint(ILogger<ListReportsEndpoint> logger)
    {
        _logger = logger;
    }

    [WolverineGet("reports")]
    public async static Task<IResult> ListReportsAsync(int page, int pageSize, IMessageBus sender, ILogger<ListReportsEndpoint> logger)
    {
        logger.LogInformation("Received request to list reports. Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var pagingResult = Paging.Create(page, pageSize);
        if (pagingResult.IsError)
        {
            logger.LogWarning("Invalid paging parameters. Errors: {Errors}", pagingResult.Errors);
            return Results.BadRequest(pagingResult.Errors.Select(e => e.Code));
        }

        var command = new ListReportsCommand(pagingResult.Value);

        var result = await sender.InvokeAsync<ErrorOr<ListReportsCommand.Result>>(command);

        return result.Match(
            value =>
            {
                logger.LogInformation("Reports retrieved successfully. Count: {Count}", value.Reports.Count());
                return Results.Ok(
                    new Response(
                        value.Reports.Select(report => new Response.Report(report.Id, report.CreatedAt))
                    )
                );
            },
            errors =>
            {
                logger.LogWarning("Failed to retrieve reports. Errors: {Errors}", errors);
                return Results.BadRequest(errors.Select(e => e.Code));
            }
        );
    }

    public record Response(IEnumerable<Response.Report> Reports)
    {
        public record Report(Guid Id, DateTime CreatedAt);
    }
}
