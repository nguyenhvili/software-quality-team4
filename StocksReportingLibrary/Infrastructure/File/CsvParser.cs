using CsvHelper.Configuration;
using CsvHelper;
using ErrorOr;
using System.Globalization;
using StocksReportingLibrary.Domain.Report.Holding;
using StocksReportingLibrary.Application.Services.File;

namespace StocksReportingLibrary.Infrastructure.File;

public class CsvParser : Application.Services.File.IParser
{
    private readonly IDownloader _downloader;

    public CsvParser(IDownloader downloader)
    {
        _downloader = downloader;
    }

    public async Task<ErrorOr<List<Holding>>> ParseAsync(string csvUrl)
    {
        var stringReaderResult = await _downloader.DownloadAsync(csvUrl);
        if (stringReaderResult.IsError)
        {
            return stringReaderResult.Errors;
        }
        var stringReader = stringReaderResult.Value;
        var holdings = new List<Holding>();
        var csv = new CsvReader(stringReader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<HoldingMap>();

        var parsedRecords = csv.GetRecordsAsync<ParsedHolding>();
        await foreach (var record in parsedRecords)
        {
            var holdingResult = Holding.Create(record.Company, record.Ticker, record.Shares, 0, record.Weight);
            if (holdingResult.IsError)
            {
                return holdingResult.Errors;
            }
            holdings.Add(holdingResult.Value);
        }

        return holdings;
    }

    private sealed class HoldingMap : ClassMap<ParsedHolding>
    {
        public HoldingMap()
        {
            Map(m => m.Company).Name("company");
            Map(m => m.Ticker).Name("ticker");
            Map(m => m.Shares).Name("shares").Convert(args =>
                long.Parse(args.Row.GetField("shares").Replace(",", ""))
            );
            Map(m => m.Weight).Name("weight (%)").Convert(args =>
                decimal.Parse(args.Row.GetField("weight (%)").Replace("%", ""), CultureInfo.InvariantCulture)
            );
        }
    }

    private record ParsedHolding
    {
        public string Company { get; set; }
        public string Ticker { get; set; }
        public long Shares { get; set; }
        public decimal Weight { get; set; }

        public ParsedHolding() { }

        public ParsedHolding(string company, string ticker, long shares, decimal weight)
        {
            Company = company;
            Ticker = ticker;
            Shares = shares;
            Weight = weight;
        }
    }
}
