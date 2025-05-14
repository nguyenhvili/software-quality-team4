using ErrorOr;
using StocksReportingLibrary.Application.Services.File;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Report.Holding;
using StocksReportingLibrary.Domain.Report.Holding.ValueObjects;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Report;

public class CreateReportCommandHandler
{
    private readonly IRepository<Domain.Report.Report> _repository;
    private readonly IQueryObject<Domain.Report.Report> _queryObject;
    private readonly IParser _csvParser;
    private readonly IWriter _csvWriter;
    private readonly ILogger<CreateReportCommandHandler> _logger;

    public CreateReportCommandHandler(
        IRepository<Domain.Report.Report> repository,
        IQueryObject<Domain.Report.Report> queryObject,
        IWriter csvWriter,
        IParser csvParser,
        ILogger<CreateReportCommandHandler> logger)
    {
        _repository = repository;
        _queryObject = queryObject;
        _csvParser = csvParser;
        _csvWriter = csvWriter;
        _logger = logger;
    }

    private void CompareHoldings(IEnumerable<Holding> previous, IEnumerable<Holding> current)
    {
        foreach (var holding in current)
        {
            var previousHolding = previous.FirstOrDefault(previous => previous.Company.Name.Equals(holding.Company.Name));

            if (previousHolding is not null && previousHolding.Shares.Value > 0)
            {
                holding.UpdateSharesPercent(SharesPercent.Create(Math.Round(((holding.Shares.Value - previousHolding.Shares.Value) / (decimal)previousHolding.Shares.Value) * 100, 2)));
            }
        }
    }

    public async Task<ErrorOr<CreateReportCommand.Result>> Handle(CreateReportCommand command)
    {
        _logger.LogInformation("CreateReportCommandHandler for DownloadPath: {DownloadPath}, CreatedAt: {CreatedAt}",
            command.DownloadPath, command.CreatedAt);

        var latestReport = (await _queryObject.OrderBy(report => report.CreatedAt, false).ExecuteAsync()).FirstOrDefault();

        var holdingsResult = await _csvParser.ParseAsync(command.DownloadPath);
        if (holdingsResult.IsError)
        {
            _logger.LogError("Failed to parse CSV: {Errors}", holdingsResult.Errors);
            return holdingsResult.Errors;
        }
        var holdings = holdingsResult.Value;

        if (latestReport is not null)
        {
            CompareHoldings(latestReport.Holdings, holdings);
            _logger.LogDebug("Compared current holdings with previous report.");
        }
        else
        {
            _logger.LogDebug("No previous report found.");
        }

        var folderPath = "GeneratedReports";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            _logger.LogDebug("Created directory: {FolderPath}", folderPath);
        }

        string fileName = $"{command.CreatedAt:yyyy-MM-dd}_stocks_report.csv";
        string filePath = Path.Combine(folderPath, fileName);

        var reportResult = Domain.Report.Report.Create(
            filePath,
            holdings,
            DateTime.UtcNow
        );

        if (reportResult.IsError)
        {
            _logger.LogError("Failed to create report: {Errors}", reportResult.Errors);
            return reportResult.Errors;
        }

        var report = reportResult.Value;

        _csvWriter.Write(report);
        _logger.LogDebug("Report written to CSV file: {FilePath}", filePath);

        await _repository.InsertAsync(report);
        await _repository.CommitAsync();

        _logger.LogInformation("Report created successfully with ID: {ReportId}, FilePath: {FilePath}",
            report.Id.Value, report.ReportPathValue.PathValue);

        return new CreateReportCommand.Result(
            new CreateReportCommand.CreatedReport(
                report.Id.Value,
                report.ReportPathValue.PathValue,
                report.CreatedAt
            )
        );
    }
}
