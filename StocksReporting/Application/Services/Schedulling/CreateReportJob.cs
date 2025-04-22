using Quartz;
using StocksReporting.Application.Report;

namespace StocksReporting.Application.Services.Schedulling;

public class CreateReportJob : IJob
{
    private readonly CreateReportCommandHandler _handler;

    private readonly string _downloadPath = "https://assets.ark-funds.com/fund-documents/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";

    public CreateReportJob(CreateReportCommandHandler handler)
    {
        _handler = handler;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await _handler.Handle(new CreateReportCommand(_downloadPath, DateTime.Now));
    }
}
