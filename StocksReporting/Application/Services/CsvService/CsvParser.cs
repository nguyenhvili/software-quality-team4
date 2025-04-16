using StocksReporting.Domain.Report.Holding;
using System.Net.Http;
using ErrorOr;
using System.Net.Security;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace StocksReporting.Application.Services.CsvService;

public class CsvParser
{
    private readonly HttpClient httpClient;

    public CsvParser(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ErrorOr<List<Holding>>> Parse(string csvUrl)
    {
        var holdings = new List<Holding>();
        using (var response = await httpClient.GetAsync(csvUrl))
        using (var stream = await response.Content.ReadAsStreamAsync())
        {
            using (var reader = new StreamReader(stream))

            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    if (csv.Parser.Count != csv.HeaderRecord?.Length)
                        continue;

                    string company = csv.GetField("company");
                    string ticker = csv.GetField("ticker");
                    long shares = long.Parse(csv.GetField("shares").Replace(",", ""));
                    decimal weight = decimal.Parse(csv.GetField("weight (%)").Replace("%", ""), CultureInfo.InvariantCulture);
                    var holdingResult = Holding.Create(company, ticker, shares, 0, weight);
                    if (holdingResult.IsError)
                    {
                        return holdingResult.Errors;
                    }
                    holdings.Add(holdingResult.Value);
                }
            }
        }
        return holdings;
    }
}
