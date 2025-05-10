using StocksReportingLibrary.Application.Services.File;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.File;

public class CsvWriter : IWriter
{
    private readonly ILogger<CsvWriter> _logger;

    public CsvWriter(ILogger<CsvWriter> logger)
    {
        _logger = logger;
    }

    public void Write(Domain.Report.Report report)
    {
        if (report is null)
        {
            _logger.LogWarning("Report is null. Nothing to write.");
            return;
        }

        var path = report.ReportPathValue.PathValue;
        var holdings = report.Holdings;

        _logger.LogInformation("Writing report to CSV file: {FilePath}", path);

        try
        {
            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            using var csv = new CsvHelper.CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteField("company");
            csv.WriteField("ticker");
            csv.WriteField("shares (%)");
            csv.WriteField("weight (%)");
            csv.NextRecord();

            foreach (var holding in holdings)
            {
                var company = holding.Company.Name;
                var ticker = holding.Ticker.Value;
                var shares = holding.Shares.Value;
                var sharesPercent = holding.SharesPercent?.Value ?? 0;
                var weight = holding.Weight.Value;

                string formattedShares;
                if (sharesPercent > 0)
                {
                    formattedShares = $"{shares} (🔺{sharesPercent}%)";
                }
                else if (sharesPercent < 0)
                {
                    formattedShares = $"{shares} (🔻{Math.Abs(sharesPercent)}%)";
                }
                else
                {
                    formattedShares = shares.ToString();
                }

                csv.WriteField(company);
                csv.WriteField(ticker);
                csv.WriteField(formattedShares);
                csv.WriteField($"{weight} %");
                csv.NextRecord();
            }

            _logger.LogInformation("Report written to CSV file successfully: {FilePath}", path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error writing report to CSV file: {FilePath}", path);
            throw;
        }
    }
}
