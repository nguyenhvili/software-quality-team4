using ErrorOr;
using StocksReportingLibrary.Application.Services.Persistence;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Report;

public class ListReportsCommandHandler
{
    private readonly IQueryObject<Domain.Report.Report> _queryObject;
    private readonly ILogger<ListReportsCommandHandler> _logger;

    public ListReportsCommandHandler(
        IQueryObject<Domain.Report.Report> queryObject,
        ILogger<ListReportsCommandHandler> logger)
    {
        _queryObject = queryObject;
        _logger = logger;
    }

    public async Task<ErrorOr<ListReportsCommand.Result>> Handle(ListReportsCommand command)
    {
        _logger.LogInformation("ListReportsCommandHandler with Page: {Page}, PageSize: {PageSize}",
            command.Paging.Page, command.Paging.PageSize);

        var reports = await _queryObject.OrderBy(r => r.CreatedAt, false).Page(command.Paging).ExecuteAsync();
        var result = reports.Select(r => new ListReportsCommand.Report(r.Id.Value, r.CreatedAt));

        _logger.LogInformation("Retrieved {ReportCount} reports.", reports.Count());
        return new ListReportsCommand.Result(result);
    }
}
