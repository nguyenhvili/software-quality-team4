using StocksReportingLibrary.Application.Services.File;
using System.Globalization;
using System.Text;

namespace StocksReportingLibrary.Infrastructure.File;

public class CsvWriter : IWriter
{
    public void Write(Domain.Report.Report report)
    {
        if (report is null)
        {
            return;
        }
        var path = report.ReportPathValue.PathValue;
        var holdings = report.Holdings;

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
    }
}
