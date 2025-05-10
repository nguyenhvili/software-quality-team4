using ErrorOr;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Report.ValueObjects;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Report;

public class GetReportCommandHandler
{
    private readonly IQueryObject<Domain.Report.Report> _queryObject;
    private readonly ILogger<GetReportCommandHandler> _logger;

    public GetReportCommandHandler(
        IQueryObject<Domain.Report.Report> queryObject,
        ILogger<GetReportCommandHandler> logger)
    {
        _queryObject = queryObject;
        _logger = logger;
    }

    public async Task<ErrorOr<GetReportCommand.Result>> Handle(GetReportCommand command)
    {
        _logger.LogInformation("GetReportCommandHandler for ReportId: {ReportId}", command.Id);

        var report = (await _queryObject.Filter(r => r.Id == ReportId.Create(command.Id)).ExecuteAsync())
            .SingleOrDefault();
        if (report is null)
        {
            _logger.LogWarning("Report with ID {ReportId} not found.", command.Id);
            return Error.NotFound("Report not found.");
        }

        var reportDetail = new GetReportCommand.ReportDetail(
            report.Id.Value,
            report.ReportPathValue.PathValue,
            report.CreatedAt,
            report.Holdings.Select(h => new GetReportCommand.HoldingDetail(h.Id.Value, h.Company.Name, h.Ticker.Value,
                h.Shares.Value, h.SharesPercent.Value, h.Weight.Value)
            ));

        _logger.LogInformation("Successfully retrieved report with ID: {ReportId}", command.Id);
        return new GetReportCommand.Result(reportDetail);
    }
}
