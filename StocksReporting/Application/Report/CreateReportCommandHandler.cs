using ErrorOr;
using StocksReporting.Application.Services;
using StocksReporting.Application.Services.CsvService;
using StocksReporting.Domain.Report.Holding;
using StocksReporting.Domain.Report.Holding.ValueObjects;

namespace StocksReporting.Application.Report;

public class CreateReportCommandHandler
{
    private readonly IRepository<Domain.Report.Report> _repository;
    private readonly IQueryObject<Domain.Report.Report> _queryObject;
    private readonly CsvParser _csvParser;
    private readonly CsvWriter _csvWriter;

    public CreateReportCommandHandler(
        IRepository<Domain.Report.Report> repository,
        IQueryObject<Domain.Report.Report> queryObject,
        CsvParser csvParser, CsvWriter csvWriter)
    {
        _repository = repository;
        _queryObject = queryObject;
        _csvParser = csvParser;
        _csvWriter = csvWriter;
    }

    private void CompareHoldings(IEnumerable<Holding> previous, IEnumerable<Holding> current)
    {
        foreach (var holding in current)
        {
            var previousHolding = previous.FirstOrDefault(previous => previous.Company.Name.Equals(holding.Company.Name));

            if (previousHolding is not null && previousHolding.Shares.Value > 0)
            {
                holding.UpdateSharesPercent( SharesPercent.Create( Math.Round(((holding.Shares.Value - previousHolding.Shares.Value) / (decimal)previousHolding.Shares.Value) * 100, 2) ) );
            }
        }
    }

    public async Task<ErrorOr<CreateReportCommand.Result>> Handle(CreateReportCommand command)
    {
        var latestReport = (await _queryObject.OrderBy(report => report.CreatedAt, false).ExecuteAsync()).FirstOrDefault();

        var holdingsResult = await _csvParser.Parse(command.DownloadPath);
        if (holdingsResult.IsError)
        {
            return holdingsResult.Errors;
        }
        var holdings = holdingsResult.Value;

        if (latestReport is not null)
        {
            CompareHoldings(latestReport.Holdings, holdings);
        }

        var folderPath = "GeneratedReports";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        // For testing purposes, the path contains Guid as unique file identifier
        string fileName = $"{command.CreatedAt:yyyy-MM-dd}_{Guid.NewGuid()}_stocks_report.csv";
        //string fileName = $"{command.CreatedAt:yyyy-MM-dd}_stocks_report.csv";
        string filePath = Path.Combine(folderPath, fileName);

        var reportResult = Domain.Report.Report.Create(
            filePath,
            holdings,
            DateTime.UtcNow
        );

        if (reportResult.IsError)
        {
            return reportResult.Errors;
        }

        var report = reportResult.Value;

        _csvWriter.Write(report);

        await _repository.InsertAsync(report);
        await _repository.CommitAsync();

        return new CreateReportCommand.Result(
            new CreateReportCommand.CreatedReport(
                report.Id.Value,
                report.ReportPathValue.PathValue,
                report.CreatedAt
            )
        );
    }
}
