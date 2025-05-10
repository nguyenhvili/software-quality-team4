using CsvHelper.Configuration;
using CsvHelper;
using ErrorOr;
using System.Globalization;
using StocksReportingLibrary.Domain.Report.Holding;
using StocksReportingLibrary.Application.Services.File;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.File;

public class CsvParser : Application.Services.File.IParser
{
    private readonly IDownloader _downloader;
    private readonly ILogger<CsvParser> _logger;

    public CsvParser(IDownloader downloader, ILogger<CsvParser> logger)
    {
        _downloader = downloader;
        _logger = logger;
    }

    public async Task<ErrorOr<List<Holding>>> ParseAsync(string csvUrl)
    {
        _logger.LogInformation("Parsing CSV from: {CsvUrl}", csvUrl);

        var stringReaderResult = await _downloader.DownloadAsync(csvUrl);
        if (stringReaderResult.IsError)
        {
            _logger.LogError("CSV download failed: {Errors}", stringReaderResult.Errors);
            return stringReaderResult.Errors;
        }
        var stringReader = stringReaderResult.Value;
        var holdings = new List<Holding>();
        var csv = new CsvReader(stringReader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<HoldingMap>();

        try
        {
            var parsedRecords = csv.GetRecordsAsync<ParsedHolding>();
            await foreach (var record in parsedRecords)
            {
                var holdingResult = Holding.Create(record.Company, record.Ticker, record.Shares, 0, record.Weight);
                if (holdingResult.IsError)
                {
                    _logger.LogError("Failed to create Holding object: {Errors}", holdingResult.Errors);
                    return holdingResult.Errors;
                }
                holdings.Add(holdingResult.Value);
            }

            _logger.LogInformation("CSV parsed successfully. {HoldingCount} holdings found.", holdings.Count);
            return holdings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing CSV: {CsvUrl}", csvUrl);
            return Error.Unexpected($"Error parsing CSV: {ex.Message}");
        }
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
