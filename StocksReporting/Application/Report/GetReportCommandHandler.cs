using ErrorOr;
using StocksReporting.Application.Services;
using StocksReporting.Domain.Report.ValueObjects;

namespace StocksReporting.Application.Report;

public class GetReportCommandHandler(IQueryObject<Domain.Report.Report> queryObject)
{
    public async Task<ErrorOr<GetReportCommand.Result>> Handle(GetReportCommand command)
    {
        var report = (await queryObject.Filter(r => r.Id == ReportId.Create(command.Id)).ExecuteAsync())
            .SingleOrDefault();
        if (report is null)
        {  
            return Error.NotFound("Report not found.");
        }

        var reportDetail = new GetReportCommand.ReportDetail(
            report.Id.Value,
            report.ReportPathValue.PathValue,
            report.CreatedAt,
            report.Holdings.Select(h => new GetReportCommand.HoldingDetail(h.Id.Value, h.Company.Name, h.Ticker.Value,
                h.Shares.Value, h.SharesPercent.Value, h.Weight.Value)
            ));

        return new GetReportCommand.Result(reportDetail);
    }
}
